//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OfferWebSystem.EFnClass
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class OfferLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OfferLocation()
        {
            this.OfferCatLocMaps = new HashSet<OfferCatLocMap>();
            this.OfferLocOutletMaps = new HashSet<OfferLocOutletMap>();
            this.OffersInfoes = new HashSet<OffersInfo>();
        }
    
        public long ID { get; set; }
        [Required(ErrorMessage = "Enter Location Name")]
        public string LocationName { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferCatLocMap> OfferCatLocMaps { get; set; }
        public virtual SysUser SysUser { get; set; }
        public virtual SysUser SysUser1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferLocOutletMap> OfferLocOutletMaps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OffersInfo> OffersInfoes { get; set; }
    }
}
