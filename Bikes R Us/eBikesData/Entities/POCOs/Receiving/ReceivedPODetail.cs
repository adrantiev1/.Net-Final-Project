using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs.Receiving
{
    public class ReceivedPODetail
    {
        public int POid { get; set; }
        public int PODetailID { get; set; }
        public int PartID { get; set; }
        public int QtyReceiving { get; set; }
        public int QtyReturning { get; set; }
        public string ReturnReason { get; set; }
    }
}
