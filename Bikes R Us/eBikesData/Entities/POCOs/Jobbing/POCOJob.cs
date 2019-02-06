using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs.Jobbing
{
    public class POCOJob
    {
        public int JobID { get; set; }

        public string JobDateIn { get; set; }

        public string JobDateStarted { get; set; }

        public string JobDateDone { get; set; }

        public string CustomerFullName { get; set; }

        public string Phone { get; set; }

        public decimal ShopRate { get; set; }

        public char StatusCode { get; set; }

        public string VehicleIdentification { get; set; }

        public int CustomerID { get; set; }

        public int  EmployeeID { get; set; }
               

    }
}
