
namespace Domain.DTO.Auth
{
	public class AuthResponseDto
	{
		public string Username { get; set; }
		public string Token { get; set; }
		public List<string> Roles { get; set; }
	}
}
