namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class ServiceDetailPart
    {
        public int ServiceDetailPartID { get; set; }

        public int ServiceDetailID { get; set; }

        public int PartID { get; set; }

        public short Quantity { get; set; }

        [Column(TypeName = "smallmoney")]
        public decimal SellingPrice { get; set; }

        public virtual Part Part { get; set; }

        public virtual ServiceDetail ServiceDetail { get; set; }
    }
}
