using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs.Receiving
{
    public class OutsandingOrder
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorPhone { get; set; }
        public int POid { get; set; }
        public int PONumber { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
