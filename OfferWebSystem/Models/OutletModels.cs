using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OfferWebSystem.EFnClass;

namespace OfferWebSystem.Models
{
    public class OutletDetailModel
    {
        public bool IsNew { get;  set ; }
        public OfferLocOutletMap Outlet { get; set; }

    }

    public class OutletListModel
    {
        public bool IsUserAdmin { get; set; }
        public List<OfferLocOutletMap> OutletList { get; set; }

    }
    

}