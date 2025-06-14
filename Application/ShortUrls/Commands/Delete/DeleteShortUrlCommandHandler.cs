using Application.Base.Handlers;
using Application.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.ShortUrls.Commands.Delete;

public class DeleteShortUrlCommandHandler : BaseHandler<DeleteShortUrlCommand, bool>
{
	private readonly IShortUrlRepository _repo;

	public DeleteShortUrlCommandHandler(IShortUrlRepository repo, ILogger<DeleteShortUrlCommandHandler> logger)
		: base(logger) => _repo = repo;

	protected override async Task<bool> HandleInternal(DeleteShortUrlCommand request, CancellationToken cancellationToken)
	{
		var url = await _repo.GetByIdAsync(request.Id);
		if (url == null)
		{
			return false;
		}

		if (!request.IsAdmin && url.CreatedById != request.RequestingUserId)
		{
			throw new UnauthorizedAccessException("You are not allowed to delete this record");
		}

		await _repo.DeleteAsync(url);
		return true;
	}
}