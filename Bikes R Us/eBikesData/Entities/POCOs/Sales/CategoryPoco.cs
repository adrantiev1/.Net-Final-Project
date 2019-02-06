using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs.Sales
{
    public class CategoryPoco
    {
        public int CategoryID { get; set; }
        public string CategoryDesc { get; set; }

        public int CategoryCount { get; set; }
    }
}
