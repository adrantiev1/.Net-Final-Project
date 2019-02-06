using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eBikesWebApp.Sales
{
    public partial class CartCustomerDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        

        protected void BackToReview_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/CartPreview.aspx", true);
        }

        protected void ContinueShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/OnlineShoppingAndCheckout.aspx", true);
        }

        //Navigation bulletList
        protected void ContinueToPurchase_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/CartFinalizeSale.aspx", true);
        }
        protected void ViewCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/CartPreview.aspx", true);
        }

        protected void PurchaseInfo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/CartCustomerDetails.aspx", true);
        }

        protected void PlaceOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/CartFinalizeSale.aspx", true);
        }
    }
}