using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class ShortUrlRepository : IShortUrlRepository
	{
		private readonly DataContext _context;

		public ShortUrlRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ShortUrl>> GetAllAsync() =>
			await _context.ShortUrls.Include(x => x.CreatedBy).ToListAsync();

		public async Task<ShortUrl> GetByIdAsync(int id) =>
			await _context.ShortUrls.Include(x => x.CreatedBy).FirstOrDefaultAsync(x => x.Id == id);

		public async Task<ShortUrl> GetByShortCodeAsync(string code) =>
			await _context.ShortUrls.FirstOrDefaultAsync(x => x.ShortCode == code);

		public async Task<ShortUrl> GetByOriginalUrlAsync(string url) =>
			await _context.ShortUrls.FirstOrDefaultAsync(x => x.OriginalUrl == url);

		public async Task AddAsync(ShortUrl url)
		{
			_context.ShortUrls.Add(url);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(ShortUrl url)
		{
			_context.ShortUrls.Remove(url);
			await _context.SaveChangesAsync();
		}
	}
}
