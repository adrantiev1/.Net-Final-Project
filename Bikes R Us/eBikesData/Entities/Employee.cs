namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            Jobs = new HashSet<Job>();
            PurchaseOrders = new HashSet<PurchaseOrder>();
            SaleRefunds = new HashSet<SaleRefund>();
            Sales = new HashSet<Sale>();
        }

        public int EmployeeID { get; set; }

        [Required(ErrorMessage ="SIN is required")]
        [StringLength(9, ErrorMessage = "SIN cannot be longer then 9 characters")]
        public string SocialInsuranceNumber { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(30, ErrorMessage = "LastName cannot be longer then 30 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(30, ErrorMessage = "FirstName cannot be longer then 30 characters")]
        public string FirstName { get; set; }

        [StringLength(40, ErrorMessage = "Address cannot be longer then 40 characters")]
        public string Address { get; set; }

        [StringLength(20, ErrorMessage = "City cannot be longer then 20 characters")]
        public string City { get; set; }

        [StringLength(2, ErrorMessage = "Province cannot be longer then 2 characters")]
        public string Province { get; set; }

        [StringLength(6, ErrorMessage = "PostalCode cannot be longer then 6 characters")]
        public string PostalCode { get; set; }

        [StringLength(12, ErrorMessage = "HomePhone cannot be longer then 12 characters")]
        public string HomePhone { get; set; }

        [StringLength(30, ErrorMessage = "EmailAddress cannot be longer then 30 characters")]
        public string EmailAddress { get; set; }

        public int PositionID { get; set; }

        public virtual Position Position { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleRefund> SaleRefunds { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
