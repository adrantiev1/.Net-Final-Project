using eBikesData.Entities.POCOs.Sales;
using eBikesSystem.BLL.Sales;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eBikesWebApp.Sales
{
    public partial class CartFinalizeSale : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var userName = User.Identity.Name;
            var controller = new SalesController();
            var shoppingCartId = controller.RetriveShoppingCartId(userName);
            if (shoppingCartId == 0)
            {
                Response.Redirect("~/Sales/OnlineShoppingAndCheckout.aspx", true);
            }
            else
            {


                List<ProductInCart> partsIncart = controller.ListProductsinCart(shoppingCartId);

                //check if items in stock and change accordingly the place order button text
                List<ProductInCart> unavailableParts = new List<ProductInCart>();
                string unavalableMessage = "";
                foreach (var part in partsIncart)
                {
                    if (part.StockAvailabillty != "")
                    {
                        PlaceOrder.Text = "Order In-Stock Items Only";
                        unavailableParts.Add(part);
                        unavalableMessage += $" {part.PartDesc} {part.Qty.ToString()} ";
                    }

                }
                if (unavailableParts.Count > 0)
                {
                    ExcludedPanle.Visible = true;
                    EcludedLabel.Text = "This parts would be excluded from your order price and placed as backorder:" + unavalableMessage + " pcs";

                }
                CartGridView.Visible = true;
                CartGridView.DataSource = partsIncart;
                CartGridView.DataBind();

                //calculate total price
                decimal subPrice = controller.CalculateTotal(shoppingCartId);
                decimal gst = subPrice * (decimal)0.05;
                decimal totalPrice = subPrice * (decimal)1.05;
                SubTotal.Text = string.Format("{0:C}", subPrice);
                TotalGST.Text = string.Format("{0:C}", gst);
                TotalPrice.Text = string.Format("{0:C}", totalPrice);


                //show amount of items in cart and lastUpdated date
                DateTime updatedOn = controller.UpdateDate(shoppingCartId);
                var partAmount = controller.CartPartCount().ToString();
                InfoRow.Text = ($"{partAmount} items in your cart(last updated on {String.Format("{0:MM/d/yyyy}", updatedOn)})");


            }

        }

        protected void ApplyCoupon_Click(object sender, EventArgs e)
        {

            var userName = User.Identity.Name;
            var controller = new SalesController();
            var shoppingCartId = controller.RetriveShoppingCartId(userName);
            //calculate total price
            decimal subPrice = controller.CalculateTotal(shoppingCartId);

            //coupon calculation
            int couponDiscount = controller.CouponValue(Coupon.Text);
            decimal couponPercent = Convert.ToDecimal(couponDiscount) * (decimal)0.01;
            decimal discount = couponPercent * subPrice;
            decimal gstAfterCoupon = (subPrice - discount) * (decimal)0.05;
            decimal totalAfterCoupon = (subPrice - discount) * (decimal)1.05;

            MessageUserControl.TryRun(() => {
                
                
                if (couponDiscount == 0)
                {
                    //DiscountLabel.Visible = false;
                    //Discount.Visible = false;
                    //decimal gst = subPrice * (decimal)0.05;
                    //decimal totalPrice = subPrice * (decimal)1.05;
                    //SubTotal.Text = string.Format("{0:C}", subPrice);
                    //TotalGST.Text = string.Format("{0:C}", gst);
                    //TotalPrice.Text = string.Format("{0:C}", totalPrice);
                    throw new Exception("Coupon is not valid");
                }
                else
                {
                    DiscountLabel.Visible = true;
                    Discount.Visible = true;
                    SubTotal.Text = string.Format("{0:C}", subPrice);
                    Discount.Text = string.Format("{0:C}", discount);
                    TotalGST.Text = string.Format("{0:C}", gstAfterCoupon);
                    TotalPrice.Text = string.Format("{0:C}", totalAfterCoupon);
                }

            },"Coupon Message",$"A coupon of {couponDiscount.ToString()} % discount applied to your order");


        }

        protected void PlaceOrder_Click(object sender, EventArgs e)
        {

            var controller = new SalesController();
            string userName = User.Identity.Name;
            decimal gst = decimal.Parse(TotalGST.Text, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint);
            var subTotal = decimal.Parse(SubTotal.Text, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint);


            string couponIdValue = "";
            if (Coupon.Text != "")
            {

                couponIdValue = Coupon.Text;
            }
            var shoppingCartId = controller.RetriveShoppingCartId(userName);
            var partsInCart = controller.ListProductsinCart(shoppingCartId);
            MessageUserControl.TryRun(() =>
            {
                string paymentType = "";
                if (PaymentType.SelectedValue == "")
                {
                    throw new Exception("You must choose payment type to complete the order");
                }
                else
                {
                    paymentType = PaymentType.SelectedValue == "Credit" ? "C" : "D";
                }
                //if (subTotal == 0)
                //{
                //    throw new Exception("You must have items in your cart to purchse");
                //}

                controller.PlaceOrder(
                    new SaleSummary
                    {
                        UserName = userName,
                        TaxAmount = gst,
                        SubTotal = subTotal,
                        CouponIdValue = couponIdValue,
                        PaymentType = paymentType,
                        Parts = partsInCart
                    });
                PlaceOrder.Visible = false;
                ReturnToShopping.Visible = true;


                //diable checkOut nav buttons
                DisableLinkButton(ViewCart, PurchaseInfo, PlaceOrderLink);

            }, "Place Order", "Order have been placed");

        }

        //Navigation bulletList
        protected void ContinueShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/OnlineShoppingAndCheckout.aspx", true);
        }

        protected void PurchaseInfo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/CartCustomerDetails.aspx", true);
        }

        protected void PlaceOrderLink_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/CartFinalizeSale.aspx", true);
        }

        protected void ViewCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/CartPreview.aspx", true);
        }
        //disable link buttons
        protected void DisableLinkButton(LinkButton ViewCart, LinkButton PurchaseInfo, LinkButton PlaceOrder)
        {
            ViewCart.Attributes.Remove("href");
            ViewCart.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "gray";
            ViewCart.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
            if (ViewCart.Enabled != false)
            {
                ViewCart.Enabled = false;
            }

            if (ViewCart.OnClientClick != null)
            {
                ViewCart.OnClientClick = null;
            }

            PurchaseInfo.Attributes.Remove("href");
            PurchaseInfo.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "gray";
            PurchaseInfo.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
            if (PurchaseInfo.Enabled != false)
            {
                PurchaseInfo.Enabled = false;
            }

            if (PurchaseInfo.OnClientClick != null)
            {
                PurchaseInfo.OnClientClick = null;
            }

            PlaceOrder.Attributes.Remove("href");
            PlaceOrder.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "gray";
            PlaceOrder.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
            if (PlaceOrder.Enabled != false)
            {
                PlaceOrder.Enabled = false;
            }

            if (PlaceOrder.OnClientClick != null)
            {
                PlaceOrder.OnClientClick = null;
            }
        }

        protected void ReturnToShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Sales/OnlineShoppingAndCheckout.aspx", true);
        }
    }
}