namespace Fanex.RemotingRef.Demo.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Context;
    using Fanex.NAP.Core.Models.ParameterObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;

    public class SSOController : Controller
    {
        private readonly int _timeExipration = 30;
        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;

        protected readonly int SiteId = NapSettings.Settings.SiteId;

        public SSOController(
            IUserService userService,
            ISecurityService securityService)
        {
            _userService = userService ?? new UserService();
            _securityService = securityService ?? new SecurityService();
        }

        public ActionResult Index(string userId, string onuserId, string time)
        {
            string decryptedUserId = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            string decryptedOnuserId = Cryptography.CfsDeCrypt(onuserId.Replace(" ", "+"));
            string decryptedTime = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            string currentTime = _securityService.GetTime();

            var expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(decryptedTime);
            var currentContext = new CurrentContext();

            if (expiration.TotalSeconds <= _timeExipration)
            {
                // SSO check-in
                var criteria = new SSOLoginParameter
                {
                    UserId = Convert.ToInt32(decryptedUserId),
                    SiteId = SiteId,
                    CheckOnUserId = Convert.ToInt32(decryptedOnuserId),
                    OnUserId = 0,
                    IP = currentContext.IP,
                    NetSessionId = Session.SessionID,
                    LoginDate = DateTime.Now,
                    BrowserName = currentContext.BrowserName,
                    CountryCode = currentContext.CountryCode,
                    Domain = currentContext.DomainName,
                    Platform = currentContext.Platform
                };

                var onlineUserId = _securityService.SSOLogin(criteria);

                if (onlineUserId != 0)
                {
                    return Redirect(FormsAuthentication.DefaultUrl);
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}