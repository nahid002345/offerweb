using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OfferWebSystem.EFnClass;

namespace OfferWebSystem.Models
{
    public class LocationDetailModel
    {
        public bool IsNew { get;  set ; }
        public OfferLocation Location { get; set; }

    }
    

}