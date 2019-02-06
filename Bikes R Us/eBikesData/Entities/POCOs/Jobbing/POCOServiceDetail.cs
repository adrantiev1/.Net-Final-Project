using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs.Jobbing
{
    public class POCOServiceDetail
    {
        public string Description { get; set; }

        public decimal JobHours { get; set; }

        public string Comments { get; set; }

        public int? CouponID { get; set; }

        public int ServiceDetailID { get; set; }

        public string CouponIDValue { get; set; }

        public int JobID { get; set; }

        public string Status { get; set; }
    }
}
