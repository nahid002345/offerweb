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
    public partial class OfferLocOutletMap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OfferLocOutletMap()
        {
            this.OfferAvailOutlets = new HashSet<OfferAvailOutlet>();
        }
    
        public long ID { get; set; }
        public long UserId { get; set; }
        public long LocationID { get; set; }
        [Required(ErrorMessage = "Enter Outlet Name")]
        public string OutletName { get; set; }
        [Required(ErrorMessage = "Enter Outlet Address Name")]
        public string OutletAddress { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferAvailOutlet> OfferAvailOutlets { get; set; }
        public virtual OfferLocation OfferLocation { get; set; }
        public virtual SysUser SysUser { get; set; }
        public virtual SysUser SysUser1 { get; set; }
    }
}
