namespace Fanex.RemotingRef.Demo.Filters
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RefAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var token = filterContext.HttpContext.Request.QueryString["token"];

            if (!string.IsNullOrEmpty(token) && IsValidToken(token))
            {
                var ticket = new FormsAuthenticationTicket(1, token, DateTime.Now, DateTime.Now.AddMinutes(10), true, token);
                var cookieData = FormsAuthentication.Encrypt(ticket);
                filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, cookieData));

                return;
            }

            filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl ?? "~/Error/Unauthorized");
        }

        private static bool IsValidToken(string token)
        {
            var decryptedToken = Cryptography.CfsDeCrypt(token.Replace(" ", "+"));
            var decryptedTime = Convert.ToDateTime(decryptedToken);

            return (DateTime.Now - decryptedTime) < TimeSpan.FromSeconds(10);
        }
    }
}