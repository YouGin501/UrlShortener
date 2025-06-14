using MediatR;

namespace Application.ShortUrls.Commands.Delete;

public record DeleteShortUrlCommand(int Id, string? RequestingUserId, bool IsAdmin) : IRequest<bool>;