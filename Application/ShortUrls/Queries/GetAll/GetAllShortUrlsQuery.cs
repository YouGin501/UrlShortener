using Domain.Entities;
using MediatR;

namespace Application.ShortUrls.Queries.GetAll;

public record GetAllShortUrlsQuery() : IRequest<IEnumerable<ShortUrl>>;