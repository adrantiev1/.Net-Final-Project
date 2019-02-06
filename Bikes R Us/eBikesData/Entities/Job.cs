namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class Job
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            ServiceDetails = new HashSet<ServiceDetail>();
            Sales = new HashSet<Sale>();
        }

        public int JobID { get; set; }

        public DateTime JobDateIn { get; set; }

        public DateTime? JobDateStarted { get; set; }

        public DateTime? JobDateDone { get; set; }

        public DateTime? JobDateOut { get; set; }

        public int CustomerID { get; set; }

        public int EmployeeID { get; set; }

        public decimal ShopRate { get; set; }

        [Required(ErrorMessage ="StatusCode is required")]
        [StringLength(1, ErrorMessage = "StatusCode cannot be longer then 1 characters")]
        public string StatusCode { get; set; }

        [Required(ErrorMessage = "VehicleIdentification is required")]
        [StringLength(50, ErrorMessage = "VehicleIdentification cannot be longer then 50 characters")]
        public string VehicleIdentification { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Employee Employee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceDetail> ServiceDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
