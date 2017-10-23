using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OfferWebSystem.EFnClass;

namespace OfferWebSystem.Models
{
    public class OfferPostListModel
    {
        public bool IsUserAdmin { get; set; }
        public List<OffersInfo> OffersList { get; set; }
    }

    public class CatLocAvailableOutlet
    {
        public long? offerId { get; set; }
        public long outletLocationId { get; set; }
        public long outletId { get; set; }
        public string outletName { get; set; }
        public string outletAddress { get; set; }
        public string outletLocation { get; set; }
        public bool? outletIsActive { get; set; }
    }

}