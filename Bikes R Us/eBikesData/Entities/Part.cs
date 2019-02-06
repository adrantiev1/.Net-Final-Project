namespace eBikesData.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class Part
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Part()
        {
            ServiceDetailParts = new HashSet<ServiceDetailPart>();
            PurchaseOrderDetails = new HashSet<PurchaseOrderDetail>();
            SaleDetails = new HashSet<SaleDetail>();
            SaleRefundDetails = new HashSet<SaleRefundDetail>();
            ShoppingCartItems = new HashSet<ShoppingCartItem>();
            StandardJobParts = new HashSet<StandardJobPart>();
        }

        public int PartID { get; set; }

        [Required(ErrorMessage ="Description is required")]
        [StringLength(40, ErrorMessage = "Description cannot be longer then 40 characters")]
        public string Description { get; set; }

        [Column(TypeName = "smallmoney")]
        public decimal PurchasePrice { get; set; }

        [Column(TypeName = "smallmoney")]
        public decimal SellingPrice { get; set; }

        public int QuantityOnHand { get; set; }

        public int ReorderLevel { get; set; }

        public int QuantityOnOrder { get; set; }

        public int CategoryID { get; set; }

        [Required(ErrorMessage ="Refundable is required")]
        [StringLength(1, ErrorMessage = "Refundable cannot be longer then 1 characters")]
        public string Refundable { get; set; }

        public bool Discontinued { get; set; }

        public int VendorID { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceDetailPart> ServiceDetailParts { get; set; }

        public virtual Vendor Vendor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleDetail> SaleDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleRefundDetail> SaleRefundDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StandardJobPart> StandardJobParts { get; set; }
    }
}
