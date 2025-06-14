using MediatR;
using Domain.Entities;

namespace Application.ShortUrls.Queries.GetById;

public record GetShortUrlByIdQuery(int Id) : IRequest<ShortUrl?>;