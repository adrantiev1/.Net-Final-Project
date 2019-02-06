using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs.Sales
{
    public class PartPoco
    {
        public int PartID { get; set; }
        public string PartDesc { get; set; }
        public decimal SellingPrice { get; set; }
        public int QtyOnHand { get; set; }
        public int? QtyInCart { get; set; }
        public int CategoryId { get; set; }
    }
}
