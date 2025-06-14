using Microsoft.Extensions.DependencyInjection;

namespace Application
{
	public static class DependencyInjectionRegister
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddMediatR(cfg =>
				cfg.RegisterServicesFromAssembly(typeof(DependencyInjectionRegister).Assembly));

			return services;
		}
	}
}
