using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.Register;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api.UnitTests
{
	public class AuthHandlersTests
	{
		private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
		private readonly Mock<ITokenService> _tokenServiceMock;
		private readonly Mock<ILogger<LoginUserCommandHandler>> _loginLoggerMock;
		private readonly Mock<ILogger<RegisterUserCommandHandler>> _registerLoggerMock;

		public AuthHandlersTests()
		{
			var store = new Mock<IUserStore<IdentityUser>>();
			_userManagerMock = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
			_tokenServiceMock = new Mock<ITokenService>();
			_loginLoggerMock = new Mock<ILogger<LoginUserCommandHandler>>();
			_registerLoggerMock = new Mock<ILogger<RegisterUserCommandHandler>>();
		}

		[Fact]
		public async Task LoginUserCommandHandler_ValidCredentials_ReturnsAuthResponse()
		{
			// Arrange
			var user = new IdentityUser("testuser") { Id = "user-id" };
			_userManagerMock.Setup(m => m.FindByNameAsync("testuser")).ReturnsAsync(user);
			_userManagerMock.Setup(m => m.CheckPasswordAsync(user, "password")).ReturnsAsync(true);
			_userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });
			_tokenServiceMock.Setup(t => t.CreateToken(user, It.IsAny<IList<string>>())).Returns("token123");

			var handler = new LoginUserCommandHandler(_userManagerMock.Object, _tokenServiceMock.Object, _loginLoggerMock.Object);
			var command = new LoginUserCommand("testuser", "password");

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.Equal("testuser", result.Username);
			Assert.Equal("token123", result.Token);
			Assert.Single(result.Roles);
		}

		[Fact]
		public async Task LoginUserCommandHandler_InvalidCredentials_ThrowsUnauthorized()
		{
			// Arrange
			_userManagerMock.Setup(m => m.FindByNameAsync("testuser")).ReturnsAsync((IdentityUser)null);

			var handler = new LoginUserCommandHandler(_userManagerMock.Object, _tokenServiceMock.Object, _loginLoggerMock.Object);
			var command = new LoginUserCommand("testuser", "wrongpass");

			// Act & Assert
			await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, CancellationToken.None));
		}

		[Fact]
		public async Task RegisterUserCommandHandler_NewUser_CreatesSuccessfully()
		{
			// Arrange
			_userManagerMock.Setup(m => m.FindByNameAsync("newuser")).ReturnsAsync((IdentityUser)null);
			_userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), "securepass")).ReturnsAsync(IdentityResult.Success);
			_userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<IdentityUser>(), "User")).ReturnsAsync(false);
			_userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<IdentityUser>(), "User")).ReturnsAsync(IdentityResult.Success);

			var handler = new RegisterUserCommandHandler(_userManagerMock.Object, _registerLoggerMock.Object);
			var command = new RegisterUserCommand("newuser", "securepass", "User");

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.Equal("User created successfully", result);
		}

		[Fact]
		public async Task RegisterUserCommandHandler_UserExists_ThrowsException()
		{
			// Arrange
			var existingUser = new IdentityUser("existing");
			_userManagerMock.Setup(m => m.FindByNameAsync("existing")).ReturnsAsync(existingUser);

			var handler = new RegisterUserCommandHandler(_userManagerMock.Object, _registerLoggerMock.Object);
			var command = new RegisterUserCommand("existing", "password", null);

			// Act & Assert
			await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
		}
	}
}
