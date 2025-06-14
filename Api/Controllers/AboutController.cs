using Application.About.Commands;
using Application.About.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AboutController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AboutController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var content = await _mediator.Send(new GetAboutPageQuery());
			return Ok(content);
		}

		[Authorize(Roles = "Admin")]
		[HttpPut]
		public async Task<IActionResult> Update([FromBody] string newContent)
		{
			await _mediator.Send(new UpdateAboutPageCommand(newContent));
			return Ok();
		}
	}
}
