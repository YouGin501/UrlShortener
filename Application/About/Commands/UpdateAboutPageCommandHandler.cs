using Application.Base.Handlers;
using Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.About.Commands
{
	public class UpdateAboutPageCommandHandler : BaseHandler<UpdateAboutPageCommand, Unit>
	{
		private readonly IAboutPageRepository _repo;

		public UpdateAboutPageCommandHandler(IAboutPageRepository aboutRepository, ILogger<UpdateAboutPageCommandHandler> logger)
		: base(logger)
		{
			_repo = aboutRepository;
		}

		protected override async Task<Unit> HandleInternal(UpdateAboutPageCommand request, CancellationToken cancellationToken)
		{
			await _repo.UpdateContentAsync(request.NewContent);
			return Unit.Value;
		}
	}
}