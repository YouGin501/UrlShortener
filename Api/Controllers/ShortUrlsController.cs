using Application.ShortUrls.Commands.Create;
using Application.ShortUrls.Commands.Delete;
using Application.ShortUrls.Queries.GetAll;
using Application.ShortUrls.Queries.GetById;
using Domain.DTO;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ShortUrlsController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly UserManager<IdentityUser> _userManager;
	private readonly ILogger<ShortUrlsController> _logger;

	public ShortUrlsController(
		IMediator mediator,
		UserManager<IdentityUser> userManager,
		ILogger<ShortUrlsController> logger)
	{
		_mediator = mediator;
		_userManager = userManager;
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var result = await _mediator.Send(new GetAllShortUrlsQuery());
		return Ok(result);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		var result = await _mediator.Send(new GetShortUrlByIdQuery(id));
		return result == null ? throw new CustomException() : Ok(result);
	}

	[Authorize]
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateShortUrlDto dto)
	{
		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return Unauthorized();
		}

		var shortUrl = await _mediator.Send(new CreateShortUrlCommand(dto.OriginalUrl, user.Id));
		return CreatedAtAction(nameof(GetById), new { id = shortUrl.Id }, shortUrl);
	}

	[Authorize]
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var user = await _userManager.GetUserAsync(User);
		var isAdmin = User.IsInRole("Admin");

		var deleted = await _mediator.Send(new DeleteShortUrlCommand(id, user?.Id, isAdmin));
		if (!deleted)
		{
			return NotFound();
		}

		return NoContent();
	}
}