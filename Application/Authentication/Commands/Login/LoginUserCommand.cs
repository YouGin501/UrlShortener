using Domain.DTO.Auth;
using MediatR;

namespace Application.Authentication.Commands.Login
{
	public record LoginUserCommand(string Username, string Password) : IRequest<AuthResponseDto>;
}
