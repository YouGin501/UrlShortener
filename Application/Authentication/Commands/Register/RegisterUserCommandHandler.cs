using Application.Base.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Authentication.Commands.Register
{
	public class RegisterUserCommandHandler : BaseHandler<RegisterUserCommand, string>
	{
		private readonly UserManager<IdentityUser> _userManager;

		public RegisterUserCommandHandler(UserManager<IdentityUser> userManager, ILogger<RegisterUserCommandHandler> logger)
			: base(logger) => _userManager = userManager;

		protected override bool ShouldLogInput => false;
		protected override bool ShouldLogOutput => true;

		protected override async Task<string> HandleInternal(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			var userExists = await _userManager.FindByNameAsync(request.Username);
			if (userExists != null)
			{
				throw new InvalidOperationException("User already exists");
			}

			var user = new IdentityUser { UserName = request.Username };
			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
			{
				throw new ApplicationException(string.Join("; ", result.Errors.Select(e => e.Description)));
			}

			if (!string.IsNullOrEmpty(request.Role))
			{
				var inRole = await _userManager.IsInRoleAsync(user, request.Role);
				if (!inRole)
				{
					await _userManager.AddToRoleAsync(user, request.Role);
				}
			}

			return "User created successfully";
		}
	}
}
