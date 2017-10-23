
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace OfferWebSystem.EFnClass
{
        [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
        public class CustomAuthorizeAttribute : AuthorizeAttribute
        {
            public string ResourceKey { get; set; }
            public string OperationKey { get; set; }

            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                var allowedRoles = Roles;
                var currentUser = httpContext.User.Identity;
                using (OFFERDBEntities db = new OFFERDBEntities())
                {
                    SysUser user = db.SysUsers.FirstOrDefault(u => u.UserId == currentUser.Name && u.SysEnum.EnumName.ToLower().Contains(allowedRoles));
                    if (user != null)
                        return true;
                    else
                        return false;
                }
                //return base.AuthorizeCore(httpContext);
            }
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                if (filterContext == null)
                {
                    throw new ArgumentNullException("filterContext");
                }

                if (AuthorizeCore(filterContext.HttpContext))
                {
                    SetCachePolicy(filterContext);
                }
                
                else if (filterContext.HttpContext.User.IsInRole("administrator"))
                {   
                    SetCachePolicy(filterContext);
                }
                else if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {   
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    filterContext.RouteData.Values.Add("Msg", "Not Authorized. Please Sign in again...");

                    //filterContext.Result = new HttpUnauthorizedResult();
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "SignIn", action = "NotAuthorized" })
                    );
                }
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.RouteData.Values.Add("Msg", "Not Authorized. Please Sign in again...");
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "SignIn", action = "Logout" })
                    );
                }

                else
                {
                    filterContext.RouteData.Values.Add("Msg", "Not Authorized. Please Sign in again...");
                    //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "SignIn", action = "NotAuthorized" })
                    //);
                }
            }

            protected void SetCachePolicy(AuthorizationContext filterContext)
            {
            //    HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
            ////cachePolicy.SetProxyMaxAge(new TimeSpan(0));
            //    cachePolicy.SetNoStore();
                
            }
        }

    }