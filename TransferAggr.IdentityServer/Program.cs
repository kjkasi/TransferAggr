using IdentityModel;
using IdentityServerAspNetIdentity;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TransferAggr.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();

			builder.Services.AddDbContext<ApplicationDbContext>(opt =>
			    opt.UseInMemoryDatabase("InMem"));

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
		        .AddEntityFrameworkStores<ApplicationDbContext>()
		        .AddDefaultTokenProviders();

			builder.Services
				.AddIdentityServer(opt =>
				{
					opt.Events.RaiseErrorEvents = true;
					opt.Events.RaiseInformationEvents = true;
					opt.Events.RaiseFailureEvents = true;
					opt.Events.RaiseSuccessEvents = true;

					// see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
					opt.EmitStaticAudienceClaim = true;
				})
				.AddInMemoryIdentityResources(Config.IdentityResources)
				.AddInMemoryApiScopes(Config.ApiScopes)
				.AddInMemoryClients(Config.Clients)
				.AddAspNetIdentity<ApplicationUser>()
				.AddProfileService<CustomProfileService>();


			builder.Services.AddAuthentication();

            var app = builder.Build();

			using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
				context.Database.EnsureDeleted();
				context.Database.EnsureCreated();
				//context.Database.Migrate();

				var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				var alice = userMgr.FindByNameAsync("alice").Result;
				if (alice == null)
				{
					alice = new ApplicationUser
					{
						UserName = "alice",
						Email = "AliceSmith@email.com",
						EmailConfirmed = true,
						FavoriteColor = "red",
					};
					var result = userMgr.CreateAsync(alice, "Pass123$").Result;
					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}

					result = userMgr.AddClaimsAsync(alice, new Claim[]{
							new Claim(JwtClaimTypes.Name, "Alice Smith"),
							new Claim(JwtClaimTypes.GivenName, "Alice"),
							new Claim(JwtClaimTypes.FamilyName, "Smith"),
							new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
						}).Result;
					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}
					Console.WriteLine("alice created");
				}
				else
				{
					Console.WriteLine("alice already exists");
				}

				var bob = userMgr.FindByNameAsync("bob").Result;
				if (bob == null)
				{
					bob = new ApplicationUser
					{
						UserName = "bob",
						Email = "BobSmith@email.com",
						EmailConfirmed = true,
						FavoriteColor = "blue",
					};
					var result = userMgr.CreateAsync(bob, "Pass123$").Result;
					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}

					result = userMgr.AddClaimsAsync(bob, new Claim[]{
							new Claim(JwtClaimTypes.Name, "Bob Smith"),
							new Claim(JwtClaimTypes.GivenName, "Bob"),
							new Claim(JwtClaimTypes.FamilyName, "Smith"),
							new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
							new Claim("location", "somewhere")
						}).Result;
					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}
					Console.WriteLine("bob created");
				}
				else
				{
					Console.WriteLine("bob already exists");
				}
			}

			app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();
			app.MapRazorPages().RequireAuthorization();

            app.Run();
        }
    }
}