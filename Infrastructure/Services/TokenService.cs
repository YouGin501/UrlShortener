using Application.Interfaces;
using Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

public class TokenService : ITokenService
{
	private readonly JwtOptions _jwt;

	public TokenService(IOptions<JwtOptions> options)
	{
		_jwt = options.Value;
	}

	public string CreateToken(IdentityUser user, IList<string> roles)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.Id),
			new Claim(ClaimTypes.Name, user.UserName!)
		};

		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _jwt.Issuer,
			audience: _jwt.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiresInMinutes),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}