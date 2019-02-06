using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eBikesData.Entities.POCOs.Sales;
using eBikesSystem.BLL.Sales;
using eBikesWebApp.Admin.Security;

namespace eBikesWebApp.Sales
{
    public partial class OnlineShoppingAndCheckout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SalesController controller = new SalesController();
            //CatGridView.Columns[0].HeaderText = "All";
            CatGridView.Columns[2].HeaderText = controller.PartsCount().ToString();
            // Secure access to this page
            if (!Request.IsAuthenticated
                || !User.IsInRole(Settings.CustomerRole)
              )
            {
                vieCart.Visible = false;
                List<PartPoco> parts = controller.ListAllParts();
                PartGridView.Visible = true;
                PartGridView.DataSource = parts;
                PartGridView.DataBind();
                //Response.Redirect("~", true);
            }
            else
            {
                //customer entry
                var userName = User.Identity.Name;
                //retrive shopping cart info to display current items in cart
                var shoppingCartId = controller.RetriveShoppingCartId(userName);
                List<PartPoco> parts = controller.ListAllParts(shoppingCartId);

                PartGridView.Visible = true;
                PartGridView.DataSource = parts;
                PartGridView.DataBind();
                //Response.Redirect("~", true);
            }
        }



        protected void PartGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SalesController controller = new SalesController();
            List<PartPoco> parts = controller.ListAllParts();
            PartGridView.Visible = true;
            PartGridView.PageIndex = e.NewPageIndex;
            PartGridView.DataSource = parts;   
            PartGridView.DataBind();

        }

        protected void AddToCart_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun((Northwind.UI.ProcessRequest)(() =>
            {
                if (User.IsInRole(Settings.EmployeeRole))
                {
                    throw new Exception("Employees are not allowed to shop online!");
                }
                if (User.IsInRole(Settings.CustomerRole))
                {
                    SalesController controller = new SalesController();
                    var gridView = (GridViewRow)((Button)sender).NamingContainer;
                    var userName = User.Identity.Name;
                    int partId = int.Parse((gridView.FindControl("PartID") as HiddenField).Value);
                    int qty = int.Parse((gridView.FindControl("QTY") as TextBox).Text);
                    if (qty <= 0)
                    {
                        throw new Exception("You maus add only positive amount");
                    }
                    controller.AddToCart(userName, partId, qty);

                    var shoppingCartId = controller.RetriveShoppingCartId(userName);
                    List<PartPoco> parts = controller.ListAllParts((int)shoppingCartId);
                    PartGridView.Visible = true;
                    PartGridView.DataSource = parts;
                    PartGridView.DataBind();

                }
                else
                {
                    throw new Exception("You must be a customer to make an online purchase");
                }

            }), "Added succesfully", "The items added to your cart");

        }

        protected void CategoryDesc_Click(object sender, EventArgs e)
        {
            var grvRow = (GridViewRow)((LinkButton)sender).NamingContainer;
            var catId = int.Parse((grvRow.FindControl("CategoryIdHidden") as HiddenField).Value);

            List<PartPoco> parts = null;
            MessageUserControl.TryRun((Northwind.UI.ProcessRequest)(() =>
            {
                SalesController controller = new SalesController();
                parts = controller.ListAllPartsByCatId((int)Convert.ToInt32((int)catId));
                if (parts == null)
                {
                    throw new Exception("There are no parts in this category");
                }
                else
                {
                    PartGridView.Visible = true;
                    PartGridView.DataSource = parts;
                    PartGridView.DataBind();
                }
            }));

        }

        protected void viewCart_Click(object sender, EventArgs e)
        {
            
            MessageUserControl.TryRun(() => {
                SalesController controller = new SalesController();
                var userName = User.Identity.Name;
                int shopCartId = controller.RetriveShoppingCartId(userName);
                if (shopCartId > 0)
                {
                    Response.Redirect("~/Sales/CartPreview.aspx", true);
                }
                else
                {
                    throw new Exception("You must have at least 1 item in cart to enter the shopping cart");
                }
            });
            

        }
    }
}