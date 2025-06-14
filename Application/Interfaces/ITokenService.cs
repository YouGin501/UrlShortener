using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(IdentityUser user, IList<string> roles);
	}
}
