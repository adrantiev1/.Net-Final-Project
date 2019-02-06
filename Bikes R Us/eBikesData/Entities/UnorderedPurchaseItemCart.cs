namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UnorderedPurchaseItemCart")]
    internal partial class UnorderedPurchaseItemCart
    {
        [Key]
        public int CartID { get; set; }

        public int PurchaseOrderNumber { get; set; }

        [StringLength(100, ErrorMessage = "Description cannot be longer then 100 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage ="VendorPartName is required")]
        [StringLength(50, ErrorMessage = "VendorPartName cannot be longer then 50 characters")]
        public string VendorPartNumber { get; set; }

        public int Quantity { get; set; }
    }
}
