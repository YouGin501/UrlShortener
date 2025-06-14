using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class AboutPageRepository : IAboutPageRepository
	{
		private readonly DataContext _context;

		public AboutPageRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<string> GetContentAsync()
		{
			return await _context.AboutPages
				.Select(x => x.Content)
				.FirstOrDefaultAsync() ?? "";
		}

		public async Task UpdateContentAsync(string content)
		{
			var about = await _context.AboutPages.FirstOrDefaultAsync();
			if (about == null)
			{
				about = new AboutPage { Content = content };
				_context.AboutPages.Add(about);
			}
			else
			{
				about.Content = content;
			}

			await _context.SaveChangesAsync();
		}
	}
}