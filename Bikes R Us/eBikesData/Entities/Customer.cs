namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            Jobs = new HashSet<Job>();
        }

        public int CustomerID { get; set; }

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

        [StringLength(12, ErrorMessage = "ContactPhone cannot be longer then 12 characters")]
        public string ContactPhone { get; set; }

        [StringLength(30, ErrorMessage = "EmailAddress cannot be longer then 30 characters")]
        public string EmailAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
