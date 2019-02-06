namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class PurchaseOrderDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PurchaseOrderDetail()
        {
            ReceiveOrderDetails = new HashSet<ReceiveOrderDetail>();
            ReturnedOrderDetails = new HashSet<ReturnedOrderDetail>();
        }

        public int PurchaseOrderDetailID { get; set; }

        public int PurchaseOrderID { get; set; }

        public int PartID { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal PurchasePrice { get; set; }

        [StringLength(50, ErrorMessage = "VendorPartName cannot be longer then 50 characters")]
        public string VendorPartNumber { get; set; }

        public virtual Part Part { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceiveOrderDetail> ReceiveOrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReturnedOrderDetail> ReturnedOrderDetails { get; set; }
    }
}
