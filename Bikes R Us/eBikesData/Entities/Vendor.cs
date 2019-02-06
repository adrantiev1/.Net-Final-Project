namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class Vendor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vendor()
        {
            Parts = new HashSet<Part>();
            PurchaseOrders = new HashSet<PurchaseOrder>();
        }

        public int VendorID { get; set; }

        [Required(ErrorMessage ="VendorName is required")]
        [StringLength(100, ErrorMessage = "VendorName cannot be longer then 100 characters")]
        public string VendorName { get; set; }

        [Required(ErrorMessage ="Phone is required")]
        [StringLength(12, ErrorMessage = "Phone cannot be longer then 12 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage ="Address is required")]
        [StringLength(30, ErrorMessage = "Address cannot be longer then 30 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage ="City is required")]
        [StringLength(30, ErrorMessage = "City cannot be longer then 30 characters")]
        public string City { get; set; }

        [Required(ErrorMessage ="ProvinceID is required")]
        [StringLength(2, ErrorMessage = "ProvinceID cannot be longer then 2 characters")]
        public string ProvinceID { get; set; }

        [Required(ErrorMessage ="PostalCode is required")]
        [StringLength(6, ErrorMessage = "PostalCode cannot be longer then 6 characters")]
        public string PostalCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Part> Parts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
