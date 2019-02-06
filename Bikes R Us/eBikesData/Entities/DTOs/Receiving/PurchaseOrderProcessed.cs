using eBikesData.Entities.POCOs.Receiving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.DTOs.Receiving
{
    public class PurchaseOrderProcessed
    {
        public int POid { get; set; }
        public List<ReceivedPODetail> receivedDetails { get; set; }
    }
}
