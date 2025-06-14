using Application.Base.Handlers;
using Application.Interfaces;
using Domain.DTO.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Authentication.Commands.Login
{
	public class LoginUserCommandHandler : BaseHandler<LoginUserCommand, AuthResponseDto>
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ITokenService _tokenService;

		public LoginUserCommandHandler(
			UserManager<IdentityUser> userManager,
			ITokenService tokenService,
			ILogger<LoginUserCommandHandler> logger)
			: base(logger)
		{
			_userManager = userManager;
			_tokenService = tokenService;
		}

		protected override bool ShouldLogInput => false;
		protected override bool ShouldLogOutput => false;

		protected override async Task<AuthResponseDto> HandleInternal(LoginUserCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByNameAsync(request.Username);
			if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
			{
				throw new UnauthorizedAccessException("Invalid credentials");
			}

			var roles = await _userManager.GetRolesAsync(user);
			var token = _tokenService.CreateToken(user, roles);

			return new AuthResponseDto
			{
				Username = user.UserName!,
				Roles = roles.ToList(),
				Token = token
			};
		}
	}
}
