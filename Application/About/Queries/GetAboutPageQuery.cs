using MediatR;

namespace Application.About.Queries
{
	public record GetAboutPageQuery() : IRequest<string>;
}
