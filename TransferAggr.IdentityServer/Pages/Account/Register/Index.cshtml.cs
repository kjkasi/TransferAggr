using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityModel;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IdentityServerAspNetIdentity.Pages.Register;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
	private readonly UserManager<ApplicationUser> _userManager;

	[BindProperty]
	public InputModel Input { get; set; }

	public Index(
		IIdentityServerInteractionService interaction,
		IAuthenticationSchemeProvider schemeProvider,
		IIdentityProviderStore identityProviderStore,
		IEventService events,
		UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager)
	{
		_userManager = userManager;
	}

    public async Task<IActionResult> OnGet(string ReturnUrl)
    {
        Input = new InputModel
        {
            ReturnUrl = ReturnUrl
        };

        return Page();
	}

	public async Task<IActionResult> OnPost()
    {
		if (ModelState.IsValid)
		{
			ApplicationUser user = new ApplicationUser()
			{
				UserName = Input.Username,
				Email = Input.Email,
				EmailConfirmed = true
			};
			var result = await _userManager.CreateAsync(user, Input.Password);
			if (result.Succeeded)
			{
				await _userManager.AddClaimsAsync(user, new List<Claim>
				{
					new Claim(JwtClaimTypes.Name, "Qwerty Qwerty"),
					new Claim(JwtClaimTypes.GivenName, "Qwerty"),
					new Claim(JwtClaimTypes.FamilyName, "Qwerty"),
					new Claim(JwtClaimTypes.WebSite, "http://qwerty.com")
				});

				return Redirect(Input.ReturnUrl);
			}
		}

        Input = new InputModel
        {
            ReturnUrl = Input.ReturnUrl
        };
		return Page();
	}

}