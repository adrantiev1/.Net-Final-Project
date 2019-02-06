using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs.Jobbing
{
    public class POCOSDPart
    {
        public int PartID { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int ServiceDetailID { get; set; }
        public decimal SellingPrice { get; set; }
        public int ServiceDetailPartID { get; set; }

    }
}
