using Application.Base.Handlers;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.ShortUrls.Commands.Create;

public class CreateShortUrlCommandHandler : BaseHandler<CreateShortUrlCommand, ShortUrl>
{
	private readonly IShortUrlRepository _repo;

	public CreateShortUrlCommandHandler(IShortUrlRepository repo, ILogger<CreateShortUrlCommandHandler> logger)
		: base(logger) => _repo = repo;

	protected override async Task<ShortUrl> HandleInternal(CreateShortUrlCommand request, CancellationToken cancellationToken)
	{
		if (!Uri.IsWellFormedUriString(request.OriginalUrl, UriKind.Absolute))
		{
			throw new ArgumentException("Invalid URL format");
		}

		var existing = await _repo.GetByOriginalUrlAsync(request.OriginalUrl);
		if (existing != null)
		{
			throw new InvalidOperationException("This URL already exists");
		}

		var shortUrl = new ShortUrl
		{
			OriginalUrl = request.OriginalUrl,
			ShortCode = Guid.NewGuid().ToString("N")[..8],
			CreatedById = request.UserId,
			CreatedDate = DateTime.UtcNow
		};

		await _repo.AddAsync(shortUrl);
		return shortUrl;
	}
}