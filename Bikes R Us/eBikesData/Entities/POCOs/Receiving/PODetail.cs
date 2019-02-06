using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs.Receiving
{
    public class PODetail
    {
        public int POid { get; set; }
        public int PODetailID { get; set; }
        public int PartID { get; set; }
        public string Description { get; set; }
        public int QtyOrdered { get; set; }
        public int QtyOutstanding { get; set; }
    }
}
