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
            var request = filterContext.HttpContext.Request;
            var token = request.QueryString["token"];

            //if (request.IsLocal)
            //{
                StoreCookie(filterContext, "allow");
                return;
            //}

            //if (!string.IsNullOrEmpty(token) && IsValidToken(token))
            //{
            //    StoreCookie(filterContext, token);
            //    return;
            //}

            //filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl ?? "~/Error/Unauthorized");
        }

        private static void StoreCookie(AuthorizationContext filterContext, string token)
        {
            var ticket = new FormsAuthenticationTicket(1, token, DateTime.Now, DateTime.Now.AddHours(24), true, token);
            var cookieData = FormsAuthentication.Encrypt(ticket);
            filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, cookieData));
        }

        private static bool IsValidToken(string token)
        {
            var decryptedToken = Cryptography.CfsDeCrypt(token.Replace(" ", "+"));
            var decryptedTime = Convert.ToDateTime(decryptedToken);

            return (DateTime.Now - decryptedTime) < TimeSpan.FromSeconds(10);
        }
    }
}