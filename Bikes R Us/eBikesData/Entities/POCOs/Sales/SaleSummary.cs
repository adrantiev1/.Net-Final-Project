using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs.Sales
{
    public class SaleSummary
    {
        public string UserName { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotal { get; set; }
        public string CouponIdValue { get; set; }
        public string PaymentType { get; set; }
        public List<ProductInCart> Parts { get; set; }
    }
}
