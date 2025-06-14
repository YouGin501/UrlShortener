namespace Application.Repositories
{
	public interface IAboutPageRepository
	{
		Task<string> GetContentAsync();
		Task UpdateContentAsync(string content);
	}
}