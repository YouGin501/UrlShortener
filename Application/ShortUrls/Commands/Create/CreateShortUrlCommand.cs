using MediatR;
using Domain.Entities;

namespace Application.ShortUrls.Commands.Create;

public record CreateShortUrlCommand(string OriginalUrl, string UserId) : IRequest<ShortUrl>;