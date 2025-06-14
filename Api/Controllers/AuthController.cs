using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.Register;
using Domain.DTO.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			var result = await _mediator.Send(new RegisterUserCommand(dto.Username, dto.Password, dto.Role));
			return Ok(result);
		}

		[HttpPost("login")]
		public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
		{
			var result = await _mediator.Send(new LoginUserCommand(dto.Username, dto.Password));
			return Ok(result);
		}
	}
}
