using OfferWebSystem.EFnClass;
using OfferWebSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfferWebSystem.Controllers
{
    [SessionExpire]
    public class DashboardController : AppBaseController
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            var Dboard = new DashboardModel();
            Dboard.UserDetail = (SysUser)Session["User"];
            Dboard.UserType = Dboard.UserDetail.SysEnum.EnumName.ToLower().Trim();
            var offersList = oDBContext.OffersInfoes.ToList();
            if (Dboard != null && Dboard.UserType.Contains("administrator"))
            {
                var sysUserList = oDBContext.SysUsers.ToList();
                Dboard.AdminDashboard = new AdminDashboardModel();
                Dboard.AdminDashboard.TotalUsers = sysUserList.Count.ToString();
                Dboard.AdminDashboard.TotalInActiveUser = sysUserList.Where(t => t.IsActive != true).Count().ToString();
                Dboard.AdminDashboard.TotalActiveUser = sysUserList.Where(t => t.IsActive == true).Count().ToString();
            }
            else
            {
                offersList = offersList.Where(t => t.CreatedBy == Dboard.UserDetail.ID).ToList();
            }

            
            if (offersList.Sum(x => x.OfferReviews.Sum(c => c.OfferReview1)) != 0)
            {
                double nominator = offersList.Sum(x => x.OfferReviews.Sum(c => c.OfferReview1)).GetValueOrDefault(0);
                double denominator = offersList.Sum(x => x.OfferReviews.Count(c=>c.OfferReview1 != null));
                double divResult = Math.Round(nominator / denominator,2);
                Dboard.UserTotalAverageRatings = divResult.ToString();
            }

            else
                Dboard.UserTotalAverageRatings = "-";
            Dboard.UserTotalFollow = offersList.Sum(x => x.OfferReviews.Where(c => c.OfferIsFollow == true).Count()).ToString();
            Dboard.UserTotalLike = offersList.Sum(x => x.OfferReviews.Where(c => c.OfferIsFavorite == true).Count()).ToString();
            Dboard.UserTotalPost = offersList.Count.ToString();
            Dboard.UserTotalReview = offersList.Sum(x => x.OfferReviews.Count()).ToString();
            Dboard.UserTotalView = offersList.Sum(x => x.OfferViews.Count()).ToString();

            Dboard.UserTopFollowedList = offersList.OrderByDescending(t => t.OfferReviews.Count(x => x.OfferIsFollow == true)).Take(5).ToList();
            Dboard.UserTopLikedList = offersList.OrderByDescending(t => t.OfferReviews.Count(x => x.OfferIsFavorite == true)).Take(5).ToList();
            Dboard.UserTopReviewedList = offersList.OrderByDescending(t => t.OfferReviews.Count).Take(5).ToList();
            Dboard.UserTopViewedList = offersList.OrderByDescending(t => t.OfferViews.Count).Take(5).ToList();

           
            return View(Dboard);
        }

        public ActionResult MenuDetails()
        {
            var userMenu = new UserMenuModel();
            if (Session["User"] != null)
            {
                SysUser oSysUser = (SysUser)Session["User"];
                userMenu.UserDetail = oSysUser;
                return PartialView("_UserMenuPartial", userMenu);
            }
            else
                return RedirectToAction("SignIn", "SignIn");
        }

        public ActionResult NavbarDetails()
        {
            var userMenu = new UserMenuModel();
            if (Session["User"] != null)
            {
                SysUser oSysUser = (SysUser)Session["User"];
                userMenu.UserDetail = oSysUser;
                return PartialView("_headerNavbarPartial", userMenu);
            }
            else
                return RedirectToAction("SignIn", "SignIn");
        }
    }
}