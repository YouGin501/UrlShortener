using MediatR;

namespace Application.Authentication.Commands.Register
{
	public record RegisterUserCommand(string Username, string Password, string? Role) : IRequest<string>;
}
