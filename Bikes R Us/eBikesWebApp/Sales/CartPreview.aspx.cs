using eBikesData.Entities.POCOs.Sales;
using eBikesSystem.BLL.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eBikesWebApp.Sales
{
    public partial class CartPreview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //populate with products in cart
            var userName = User.Identity.Name;
            var controller = new SalesController();
            var shoppingCartId = controller.RetriveShoppingCartId(userName);
            List<ProductInCart> partsIncart = controller.ListProductsinCart(shoppingCartId);

            CartGridView.Visible = true;
            CartGridView.DataSource = partsIncart;
            CartGridView.DataBind();

            //calculate total price
            decimal totalPrice = controller.CalculateTotal(shoppingCartId);
            TotalPrice.Text = string.Format("{0:C}", totalPrice);

            //show amount of items in cart and lastUpdated date
            DateTime updatedOn = controller.UpdateDate(shoppingCartId);
            var partAmount = controller.CartPartCount().ToString();
            InfoRow.Text = ($"{partAmount} items in your cart(last updated on {String.Format("{0:MM/d/yyyy}", updatedOn)})");

        }



        protected void DeleteItem_Click(object sender, EventArgs e)
        {
            SalesController controller = new SalesController();
            var userName = User.Identity.Name;

            MessageUserControl.TryRun(() =>
            {

                
                var gridView = (GridViewRow)((Button)sender).NamingContainer;
                int cartItemId = int.Parse((gridView.FindControl("CartItemId") as HiddenField).Value);
                controller.DeleteCartItem(cartItemId);

                var shoppingCartId = controller.RetriveShoppingCartId(userName);
                List<ProductInCart> partsIncart = controller.ListProductsinCart(shoppingCartId);

                CartGridView.Visible = true;
                CartGridView.DataSource = partsIncart;
                CartGridView.DataBind();

                //calculate total price
                decimal totalPrice = controller.CalculateTotal(shoppingCartId);
                TotalPrice.Text = string.Format("{0:C}", totalPrice);

                //show amount of items in cart and lastUpdated date
                DateTime updatedOn = controller.UpdateDate(shoppingCartId);
                var partAmount = controller.CartPartCount().ToString();
                InfoRow.Text = ($"{partAmount} items in your cart(last updated on {String.Format("{0:MM/d/yyyy}", updatedOn)})");



            }, "Successfuly deleted", $"Item have been deleted from your cart");
            
        }

        protected void UpdateItemQty_Click(object sender, EventArgs e)
        {
            SalesController controller = new SalesController();
            var userName = User.Identity.Name;
            var gridView = (GridViewRow)((Button)sender).NamingContainer;
            int cartItemId = int.Parse((gridView.FindControl("CartItemId") as HiddenField).Value);
            int qty = int.Parse((gridView.FindControl("QtyTextBox") as TextBox).Text);

            MessageUserControl.TryRun(() =>
            {
                
                if (qty <= 0)
                {
                    throw new Exception("Quantity must be positive value");
                }
                else
                {
                    controller.UpdateCartItemQty(cartItemId, qty);
                    var shoppingCartId = controller.RetriveShoppingCartId(userName);
                    List<ProductInCart> partsIncart = controller.ListProductsinCart(shoppingCartId);

                    CartGridView.Visible = true;
                    CartGridView.DataSource = partsIncart;
                    CartGridView.DataBind();
                    //calculate total price
                    decimal totalPrice = controller.CalculateTotal(shoppingCartId);
                    TotalPrice.Text = string.Format("{0:C}", totalPrice);

                    //show amount of items in cart and lastUpdated date
                    DateTime updatedOn = controller.UpdateDate(shoppingCartId);
                    var partAmount = controller.CartPartCount().ToString();
                    InfoRow.Text = ($"{partAmount} items in your cart(last updated on {String.Format("{0:MM/d/yyyy}", updatedOn)})");
                }
                
            },"Successfuly updateted",$"Quntity updated to {qty} ");
        }

        protected void ContinueToCustDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/CartCustomerDetails.aspx", true);
        }

        //Navigation bulletList
        protected void ContinueShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/OnlineShoppingAndCheckout.aspx", true);
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