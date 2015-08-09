using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MvcPhoNoombuRemake.Filters
{
    public class AuthorizeEx : AuthorizeAttribute 
    {
        public string RedirectUrl { get; set; }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(RedirectUrl))
            {
                var config = WebConfigurationManager.OpenWebConfiguration("~");
                var grp = (SystemWebSectionGroup)config.GetSectionGroup("system.web");
                if (grp != null)
                    RedirectUrl = grp.Authentication.Forms.LoginUrl;
            }
            filterContext.Result = new RedirectResult(RedirectUrl);
        }
    }
}