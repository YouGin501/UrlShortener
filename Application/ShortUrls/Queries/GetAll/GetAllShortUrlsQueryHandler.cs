using Application.Base.Handlers;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.ShortUrls.Queries.GetAll;

public class GetAllShortUrlsQueryHandler : BaseHandler<GetAllShortUrlsQuery, IEnumerable<ShortUrl>>
{
	private readonly IShortUrlRepository _repo;

	public GetAllShortUrlsQueryHandler(IShortUrlRepository repo, ILogger<GetAllShortUrlsQueryHandler> logger)
		: base(logger) => _repo = repo;

	protected override async Task<IEnumerable<ShortUrl>> HandleInternal(GetAllShortUrlsQuery request, CancellationToken cancellationToken)
		=> await _repo.GetAllAsync();
}