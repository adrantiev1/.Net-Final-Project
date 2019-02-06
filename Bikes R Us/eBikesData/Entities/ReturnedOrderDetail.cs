namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class ReturnedOrderDetail
    {
        public int ReturnedOrderDetailID { get; set; }

        public int ReceiveOrderID { get; set; }

        public int? PurchaseOrderDetailID { get; set; }

        [StringLength(50, ErrorMessage = "ItemDescription cannot be longer then 50 characters")]
        public string ItemDescription { get; set; }

        public int Quantity { get; set; }

        [Required(ErrorMessage ="Reason is required")]
        [StringLength(50, ErrorMessage = "Reason cannot be longer then 50 characters")]
        public string Reason { get; set; }

        [StringLength(50, ErrorMessage = "VendorPartName cannot be longer then 50 characters")]
        public string VendorPartNumber { get; set; }

        public virtual PurchaseOrderDetail PurchaseOrderDetail { get; set; }

        public virtual ReceiveOrder ReceiveOrder { get; set; }
    }
}
