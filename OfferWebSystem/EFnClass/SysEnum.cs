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
    
    public partial class SysEnum
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SysEnum()
        {
            this.SysUsers = new HashSet<SysUser>();
            this.OffersInfoes = new HashSet<OffersInfo>();
            this.OffersInfoes1 = new HashSet<OffersInfo>();
        }
    
        public long ID { get; set; }
        public int EnumType { get; set; }
        public string EnumName { get; set; }
        public string EnumValue { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SysUser> SysUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OffersInfo> OffersInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OffersInfo> OffersInfoes1 { get; set; }
    }
}
