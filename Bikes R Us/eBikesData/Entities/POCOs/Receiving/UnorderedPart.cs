using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs.Receiving
{
    public class UnorderedPart
    {
        public int PONumber { get; set; }
        public int CartID { get; set; }
        public string Description { get; set; }
        public string VendorPartNumber { get; set; }
        public int Quantity { get; set; }
    }
}
