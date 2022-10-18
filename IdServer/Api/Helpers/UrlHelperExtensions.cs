using IdServer.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace IdServer.Api.Helpers
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, int userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(IdentityController.ConfirmEmail),
                controller: "Identity",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
