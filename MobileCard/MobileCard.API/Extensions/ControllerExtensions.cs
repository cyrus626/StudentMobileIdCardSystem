using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MobileCard.API.Controllers.Responses;
using MobileCard.API.Models.Entities;
using System.Security.Claims;

namespace MobileCard.API.Extensions
{
    public interface IUserAwareController
    {
        ClaimsPrincipal User { get; }
        UserManager<ApplicationUser> UserManager { get; }

        UnauthorizedObjectResult Unauthorized(object value);
        UnauthorizedResult Unauthorized();
    }

    public static class ControllerExtensions
    {
        public static Task<IActionResult> GetCurrentUser(this IUserAwareController c, out ApplicationUser current)
        {
            IActionResult res = null;
            current = c.UserManager.GetUserAsync(c.User).Result;

            if (current == null) res = c.Unauthorized(AuthResponses.AccountNotFound);
            else if (current.LockoutEnabled) res = c.Unauthorized(AuthResponses.AccountLockout);

            return Task.FromResult(res);
        }

        public static string GetRootUrl(this ControllerBase c) => c.Request.Scheme + "://" + c.Request.Host.Value;
    }
}
