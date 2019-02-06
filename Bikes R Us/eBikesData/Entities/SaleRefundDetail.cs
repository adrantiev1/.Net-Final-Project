namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class SaleRefundDetail
    {
        public int SaleRefundDetailID { get; set; }

        public int SaleRefundID { get; set; }

        public int PartID { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal SellingPrice { get; set; }

        [StringLength(150, ErrorMessage = "Reason cannot be longer then 150 characters")]
        public string Reason { get; set; }

        public virtual Part Part { get; set; }

        public virtual SaleRefund SaleRefund { get; set; }
    }
}
