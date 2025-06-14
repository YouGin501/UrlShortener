using MediatR;

namespace Application.About.Commands
{
	public record UpdateAboutPageCommand(string NewContent) : IRequest<Unit>;
}
