using Application.Interfaces;
using Application.Repositories;
using Infrastructure.Data;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class DependencyInjectionRegister
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

			services.AddDbContext<DataContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
			services.AddScoped<IAboutPageRepository, AboutPageRepository>();

			services.AddScoped<ITokenService, TokenService>();

			return services;
		}
	}
}