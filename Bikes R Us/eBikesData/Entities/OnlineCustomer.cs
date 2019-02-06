namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OnlineCustomer")]
    internal partial class OnlineCustomer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OnlineCustomer()
        {
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int OnlineCustomerID { get; set; }

        [StringLength(128, ErrorMessage = "UserName cannot be longer then 128 characters")]
        public string UserName { get; set; }

        public Guid? TrackingCookie { get; set; }

        public DateTime CreatedOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
