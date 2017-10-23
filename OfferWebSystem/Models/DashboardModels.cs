using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OfferWebSystem.EFnClass;

namespace OfferWebSystem.Models
{
    public class DashboardModel
    {
        public SysUser UserDetail { get; set; }
        public string UserType { get; set; }
        public string UserTotalPost { get; set; }
        public string UserTotalReview { get; set; }
        public string UserTotalFollow { get; set; }
        public string UserTotalLike { get; set; }
        public string UserTotalView { get; set; }
        public string UserTotalAverageRatings { get; set; }

        public OfferReview UserLastReviewDetail { get; set; }
        public List<OffersInfo> UserTopReviewedList { get; set; }
        public List<OffersInfo> UserTopLikedList { get; set; }
        public List<OffersInfo> UserTopFollowedList { get; set; }
        public List<OffersInfo> UserTopViewedList { get; set; }
        public AdminDashboardModel AdminDashboard { get; set; }
    
    }

    public class AdminDashboardModel
    {
        public string TotalUsers { get; set; }
        public string TotalActiveUser { get; set; }
        public string TotalInActiveUser { get; set; }

    }
    public class UserMenuModel
    {
        public SysUser UserDetail { get; set; }
        public List<SysEnum> UserAssignedMenu { get; set; }

    }

}