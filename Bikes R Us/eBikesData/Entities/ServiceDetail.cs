namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class ServiceDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServiceDetail()
        {
            ServiceDetailParts = new HashSet<ServiceDetailPart>();
        }

        public int ServiceDetailID { get; set; }

        public int JobID { get; set; }

        [Required(ErrorMessage ="Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot be longer then 100 characters")]
        public string Description { get; set; }

        public decimal JobHours { get; set; }

        public string Comments { get; set; }

        public int? CouponID { get; set; }

        [StringLength(1, ErrorMessage = "Status cannot be longer then 1 characters")]
        public string Status { get; set; }

        public virtual Coupon Coupon { get; set; }

        public virtual Job Job { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceDetailPart> ServiceDetailParts { get; set; }
    }
}
