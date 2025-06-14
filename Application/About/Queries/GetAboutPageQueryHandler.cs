using Application.Base.Handlers;
using Application.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.About.Queries
{
	public class GetAboutPageQueryHandler : BaseHandler<GetAboutPageQuery, string>
	{
		private readonly IAboutPageRepository _repo;

		public GetAboutPageQueryHandler(IAboutPageRepository aboutRepository, ILogger<GetAboutPageQueryHandler> logger) : base(logger)
		{
			_repo = aboutRepository;
		}

		protected override async Task<string> HandleInternal(GetAboutPageQuery request, CancellationToken cancellationToken)
		{
			return await _repo.GetContentAsync();
		}
	}
}
