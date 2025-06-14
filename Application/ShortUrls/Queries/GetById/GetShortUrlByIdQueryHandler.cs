using Application.Base.Handlers;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.ShortUrls.Queries.GetById;
public class GetShortUrlByIdQueryHandler : BaseHandler<GetShortUrlByIdQuery, ShortUrl?>
{
	private readonly IShortUrlRepository _repo;

	public GetShortUrlByIdQueryHandler(IShortUrlRepository repo, ILogger<GetShortUrlByIdQueryHandler> logger)
		: base(logger) => _repo = repo;

	protected override async Task<ShortUrl?> HandleInternal(GetShortUrlByIdQuery request, CancellationToken cancellationToken)
		=> await _repo.GetByIdAsync(request.Id);
}