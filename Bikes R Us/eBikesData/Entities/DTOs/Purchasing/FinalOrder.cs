using eBikesData.Entities.POCOs.Purchasing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.DTOs.Purchasing
{
    public class FinalOrder
    {
        public int VendorID { get; set; }
        public int EmployeeID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int PurchaseOrderDetailID { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GST { get; set; }
        public decimal Total { get; set; }
        public List<CurrentOrder> OrderDetails { get; set; }
       
    }
}
