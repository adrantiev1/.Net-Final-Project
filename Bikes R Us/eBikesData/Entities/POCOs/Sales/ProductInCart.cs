using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs.Sales
{
    public class ProductInCart
    {
        public int CartItemId { get; set; }
        public string PartDesc { get; set; }
        public int Qty { get; set; }
        public decimal SellPrice { get; set; }
        public decimal ItemTotalPrice { get; set; }
        public string StockAvailabillty { get; set; }




    }
}
