using Domain.Entities;

namespace Application.Repositories
{
	public interface IShortUrlRepository
	{
		Task<IEnumerable<ShortUrl>> GetAllAsync();
		Task<ShortUrl> GetByIdAsync(int id);
		Task<ShortUrl> GetByShortCodeAsync(string code);
		Task<ShortUrl> GetByOriginalUrlAsync(string url);
		Task AddAsync(ShortUrl url);
		Task DeleteAsync(ShortUrl url);
	}
}