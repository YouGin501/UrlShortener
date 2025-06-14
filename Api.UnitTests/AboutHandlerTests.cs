using Application.About.Commands;
using Application.About.Queries;
using Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api.UnitTests
{
	public class AboutHandlersTests
	{
		private readonly Mock<IAboutPageRepository> _repoMock;
		private readonly Mock<ILogger<GetAboutPageQueryHandler>> _getLoggerMock;
		private readonly Mock<ILogger<UpdateAboutPageCommandHandler>> _updateLoggerMock;

		public AboutHandlersTests()
		{
			_repoMock = new Mock<IAboutPageRepository>();
			_getLoggerMock = new Mock<ILogger<GetAboutPageQueryHandler>>();
			_updateLoggerMock = new Mock<ILogger<UpdateAboutPageCommandHandler>>();
		}

		[Fact]
		public async Task GetAboutPageQueryHandler_Returns_Content()
		{
			// Arrange
			var expectedContent = "This is about page.";
			_repoMock.Setup(r => r.GetContentAsync()).ReturnsAsync(expectedContent);

			var handler = new GetAboutPageQueryHandler(_repoMock.Object, _getLoggerMock.Object);
			var query = new GetAboutPageQuery();

			// Act
			var result = await handler.Handle(query, CancellationToken.None);

			// Assert
			Assert.Equal(expectedContent, result);
		}

		[Fact]
		public async Task UpdateAboutPageCommandHandler_Updates_Successfully()
		{
			// Arrange
			var newContent = "Updated content.";
			_repoMock.Setup(r => r.UpdateContentAsync(newContent)).Returns(Task.CompletedTask);

			var handler = new UpdateAboutPageCommandHandler(_repoMock.Object, _updateLoggerMock.Object);
			var command = new UpdateAboutPageCommand(newContent);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.Equal(Unit.Value, result);
			_repoMock.Verify(r => r.UpdateContentAsync(newContent), Times.Once);
		}
	}
}
