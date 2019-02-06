using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eBikesWebApp.Admin.Security;
using eBikesSystem.BLL.Purchasing;
using eBikesData.Entities.POCOs.Purchasing;
using eBikesData.Entities.DTOs.Purchasing;

namespace eBikesWebApp.Purchasing
{
    public partial class CreatePurchaseOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Secure access to this page
            if (!Request.IsAuthenticated
                || !User.IsInRole(Settings.PurchasingRole)
              )
            { Response.Redirect("~", true); }
            EmployeeName.Text = User.Identity.Name;


        }
        protected void CheckForExceptions(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void GetPurchaseOrder_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                bool is_Selected = VendorDropDown.SelectedIndex > 0;

                if (is_Selected)
                {

                    //get customer summary information
                    var controller = new PurchasingController();
                    var info = controller.GetVendorSummary(int.Parse(VendorDropDown.SelectedValue));
                    VendorName.Text = info.Name;
                    Location.Text = info.Location;
                    Phone.Text = info.Phone;

                    OrderDetailsPanel.Enabled = true;
                    OrderDetailsPanel.Visible = true;


                    // Populate GridViews
                    var currentOrderDetails = controller.GetCurrentOrder((int.Parse(VendorDropDown.SelectedValue)), EmployeeName.Text);
                    CurrentOrderGridView.DataSource = currentOrderDetails;
                    CurrentOrderGridView.DataBind();

                    var currentInventoryDetails = controller.GenInventoryForVendor(int.Parse(VendorDropDown.SelectedValue));
                    CurrentInventoryGridView.DataSource = currentInventoryDetails;
                    CurrentInventoryGridView.DataBind();

                    //calculate Totals
                    CalculateTotals();


                }
                else
                {
                    throw new Exception("Please select a vendor");
                }


            }, "Success", "Order found!");


        }

        protected void CurrentOrderGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView order = e.CommandSource as GridView;
            GridView inventory = CurrentInventoryGridView;
            PullOrderRow(e, order, inventory);
        }

        private void PullOrderRow(GridViewCommandEventArgs e, GridView order, GridView inventory)
        {
            MessageUserControl.TryRun(() =>
            {
            int index;
            if (int.TryParse(e.CommandArgument.ToString(), out index))
            {
                List<CurrentOrder> inventoryDetails = new List<CurrentOrder>();
                List<CurrentOrder> orderDetails = new List<CurrentOrder>();

                foreach (GridViewRow row in CurrentOrderGridView.Rows)
                {
                    var purchaseorderid = row.FindControl("PurchaseOrderID") as HiddenField;
                    var partid = row.FindControl("PartID") as Label;
                    var desc = row.FindControl("Description") as Label;
                    var qoh = row.FindControl("QOH") as Label;
                    var qoo = row.FindControl("QOO") as Label;
                    var rol = row.FindControl("ROL") as Label;
                    var buffer = row.FindControl("Buffer") as HiddenField;
                    var qty = row.FindControl("Qty") as TextBox;
                    var price = row.FindControl("Price") as TextBox;

                    if (partid != null)
                    {
                        var details = new CurrentOrder();
                        details.PurchaseOrderID = int.Parse(purchaseorderid.Value);
                        details.PartID = int.Parse(partid.Text);
                        details.Description = desc.Text;
                        if (qoh.Text == "0")
                        {
                            details.QOH = 0;
                        }
                        else
                        {
                            details.QOH = int.Parse(qoh.Text);
                        }

                        details.QOO = int.Parse(qoo.Text);
                        details.ROL = int.Parse(rol.Text);
                        details.Qty = int.Parse(qty.Text);
                        details.Price = decimal.Parse(price.Text);
                        details.Buffer =int.Parse(buffer.Value);
                        orderDetails.Add(details);

                    }
                }

                foreach (GridViewRow row in CurrentInventoryGridView.Rows)
                {
                    var purchaseorderid = row.FindControl("PurchaseOrderID") as HiddenField;
                    var partid = row.FindControl("PartID") as Label;
                    var desc = row.FindControl("Description") as Label;
                    var qoh = row.FindControl("QOH") as Label;
                    var qoo = row.FindControl("QOO") as Label;
                    var rol = row.FindControl("ROL") as Label;
                    var price = row.FindControl("Price") as Label;
                    var buffer = row.FindControl("Buffer") as Label;

                    if (partid != null)
                    {
                        var details = new CurrentOrder();
                        details.PurchaseOrderID = int.Parse(purchaseorderid.Value);
                        details.PartID = int.Parse(partid.Text);
                        details.Description = desc.Text;
                        if (qoh.Text == "0")
                        {
                            details.QOH = 0;
                        }
                        else
                        {
                            details.QOH = int.Parse(qoh.Text);
                        }

                        details.QOO = int.Parse(qoo.Text);
                        details.ROL = int.Parse(rol.Text);
                        details.Buffer = int.Parse(buffer.Text);
                        details.Price = decimal.Parse(price.Text);

                        inventoryDetails.Add(details);

                    }
                }

                var addtoinventory = orderDetails[index];
                var currentinventory = FromOrderToInventory(inventory);
                currentinventory.Add(addtoinventory);
                CurrentInventoryGridView.DataSource = currentinventory;
                CurrentInventoryGridView.DataBind();
                //add items to inventory details
                orderDetails.RemoveAt(index);
                order.DataSource = orderDetails;
                order.DataBind();

            }
            else
            {
                throw new Exception("Product is not in the list.");
            }
            }, "Success", "Part removed from the order");
        }

        private List<CurrentOrder> FromOrderToInventory(GridView currentorder)
        {
            List<CurrentOrder> inventoryDetails = new List<CurrentOrder>();

            foreach (GridViewRow row in currentorder.Rows)
            {
                var purchaseorderid = row.FindControl("PurchaseOrderID") as HiddenField;
                var partid = row.FindControl("PartID") as Label;
                var desc = row.FindControl("Description") as Label;
                var qoh = row.FindControl("QOH") as Label;
                var qoo = row.FindControl("QOO") as Label;
                var rol = row.FindControl("ROL") as Label;
                var buffer = row.FindControl("Buffer") as Label;
                var price = row.FindControl("Price") as Label;

                var details = new CurrentOrder();
                details.PurchaseOrderID = int.Parse(purchaseorderid.Value);
                details.PartID = int.Parse(partid.Text);
                details.Description = desc.Text;
                details.QOH = int.Parse(qoh.Text);
                details.QOO = int.Parse(qoo.Text);
                details.ROL = int.Parse(rol.Text);
                details.Buffer = int.Parse(buffer.Text);
                details.Price = decimal.Parse(price.Text);

                inventoryDetails.Add(details);

            }
            return inventoryDetails;
        }

        #region Add from Inventory to Order
        protected void CurrentInventoryGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView inventory = e.CommandSource as GridView;
            GridView order = CurrentOrderGridView;
            PullInventoryRow(e, inventory, order);
        }


        private void PullInventoryRow(GridViewCommandEventArgs e, GridView inventory, GridView order)
        {
            MessageUserControl.TryRun(() =>
            {
                int index;
                if (int.TryParse(e.CommandArgument.ToString(), out index))
                {
                    List<CurrentOrder> inventoryDetails = new List<CurrentOrder>();
                    List<CurrentOrder> orderDetails = new List<CurrentOrder>();

                    foreach (GridViewRow row in CurrentOrderGridView.Rows)
                    {
                        var purchaseorderid = row.FindControl("PurchaseOrderID") as HiddenField;
                        var partid = row.FindControl("PartID") as Label;
                        var desc = row.FindControl("Description") as Label;
                        var qoh = row.FindControl("QOH") as Label;
                        var qoo = row.FindControl("QOO") as Label;
                        var rol = row.FindControl("ROL") as Label;
                        var buffer = rol.FindControl("Buffer") as HiddenField;
                        var price = row.FindControl("Price") as TextBox;

                        if (partid != null)
                        {
                            var details = new CurrentOrder();
                            details.PurchaseOrderID = int.Parse(purchaseorderid.Value);
                            details.PartID = int.Parse(partid.Text);
                            details.Description = desc.Text;
                            if (qoh.Text == "0")
                            {
                                details.QOH = 0;
                            }
                            else
                            {
                                details.QOH = int.Parse(qoh.Text);
                            }

                            details.QOO = int.Parse(qoo.Text);
                            details.ROL = int.Parse(rol.Text);
                            details.Buffer = int.Parse(buffer.Value);
                            details.Price = decimal.Parse(price.Text);

                            orderDetails.Add(details);

                        }
                    }

                    foreach (GridViewRow row in CurrentInventoryGridView.Rows)
                    {
                        var purchaseorderid = row.FindControl("PurchaseOrderID") as HiddenField;
                        var partid = row.FindControl("PartID") as Label;
                        var desc = row.FindControl("Description") as Label;
                        var qoh = row.FindControl("QOH") as Label;
                        var qoo = row.FindControl("QOO") as Label;
                        var rol = row.FindControl("ROL") as Label;
                        var buffer = row.FindControl("Buffer") as Label;
                        var price = row.FindControl("Price") as Label;

                        if (partid != null)
                        {
                            var details = new CurrentOrder();
                            details.PurchaseOrderID = int.Parse(purchaseorderid.Value);
                            details.PartID = int.Parse(partid.Text);
                            details.Description = desc.Text;
                            if (qoh.Text == "0")
                            {
                                details.QOH = 0;
                            }
                            else
                            {
                                details.QOH = int.Parse(qoh.Text);
                            }

                            details.QOO = int.Parse(qoo.Text);
                            details.ROL = int.Parse(rol.Text);
                            details.Buffer = int.Parse(buffer.Text);
                            details.Price = decimal.Parse(price.Text);

                            inventoryDetails.Add(details);

                        }
                    }

                    var addtoorder = inventoryDetails[index];
                    var coorder = FromInventoryToOrder(order);
                    coorder.Add(addtoorder);
                    CurrentOrderGridView.DataSource = coorder;
                    CurrentOrderGridView.DataBind();
                    //add items to inventory details
                    inventoryDetails.RemoveAt(index);
                    inventory.DataSource = inventoryDetails;
                    inventory.DataBind();

                }
                else
                {
                    throw new Exception("Product is not in the list.");
                }
            }, "Success", "Part added to the order");


        }

        private List<CurrentOrder> FromInventoryToOrder(GridView currentinventory)
        {
            
            List<CurrentOrder> orderDetails = new List<CurrentOrder>();

            foreach (GridViewRow row in currentinventory.Rows)
            {
                var purchaseorderid = row.FindControl("PurchaseOrderID") as HiddenField;
                var partid = row.FindControl("PartID") as Label;
                var desc = row.FindControl("Description") as Label;
                var qoh = row.FindControl("QOH") as Label;
                var qoo = row.FindControl("QOO") as Label;
                var rol = row.FindControl("ROL") as Label;
                var qty = row.FindControl("Qty") as TextBox;
                var buffer = row.FindControl("Buffer") as HiddenField;
                var price = row.FindControl("Price") as TextBox;

                var details = new CurrentOrder();
                details.PurchaseOrderID = int.Parse(purchaseorderid.Value);
                details.PartID = int.Parse(partid.Text);
                details.Description = desc.Text;
                details.QOH = int.Parse(qoh.Text);
                details.QOO = int.Parse(qoo.Text);
                details.ROL = int.Parse(rol.Text);
                details.Qty = int.Parse(qty.Text);
                details.Buffer = int.Parse(buffer.Value);
                details.Price = decimal.Parse(price.Text);

                orderDetails.Add(details);

            }
            return orderDetails;
        }
        #endregion


        protected void Clear_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                OrderDetailsPanel.Enabled = false;
                OrderDetailsPanel.Visible = false;
                VendorDropDown.SelectedIndex = 0;
                VendorName.Text = "";
                Location.Text = "";
                Phone.Text = "";
            }, "Success", "Cleared");


        }

        protected void Place_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                CalculateTotals();

                FinalOrder purchase = new FinalOrder();
                purchase.PurchaseOrderID = Convert.ToInt32((CurrentOrderGridView.Rows[0].FindControl("PurchaseOrderID") as HiddenField).Value);
                purchase.VendorID = int.Parse(VendorDropDown.SelectedValue);
                purchase.Subtotal = decimal.Parse(Subtotal.Text.Substring(1));
                purchase.GST = decimal.Parse(GST.Text.Substring(1));
                List<CurrentOrder> orderDetails = new List<CurrentOrder>();

                foreach (GridViewRow row in CurrentOrderGridView.Rows)
                {

                    var partid = row.FindControl("PartID") as Label;
                    var qty = row.FindControl("Qty") as TextBox;
                    var price = row.FindControl("Price") as TextBox;
                    if (int.Parse(qty.Text) < 0)
                    {
                        throw new Exception("Quantity cannot be a negative number");
                    }
                    if (decimal.Parse(price.Text) <= 0)
                    {
                        throw new Exception("Price cannot be a negative number");
                    }
                    if (partid != null)
                    {
                        var details = new CurrentOrder();
                        details.PartID = int.Parse(partid.Text);
                        details.Qty = int.Parse(qty.Text);
                        details.Price = decimal.Parse(price.Text);
                        orderDetails.Add(details);
                    }
                    else
                    {
                        throw new Exception("Empty");
                    }

                }

                var controller = new PurchasingController();
                purchase.OrderDetails = orderDetails;
                controller.PlaceOrder(purchase);
            }, "Sucess", "Order placed and cannot be deleted");
        }



        protected void Update_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                CalculateTotals();

                FinalOrder purchase = new FinalOrder();
                purchase.PurchaseOrderID = Convert.ToInt32((CurrentOrderGridView.Rows[0].FindControl("PurchaseOrderID") as HiddenField).Value);
                purchase.PurchaseOrderDetailID = Convert.ToInt32((CurrentOrderGridView.Rows[0].FindControl("PurchaseOrderDetailID") as HiddenField).Value);
                purchase.VendorID = int.Parse(VendorDropDown.SelectedValue);
                purchase.Subtotal = decimal.Parse(Subtotal.Text.Substring(1));
                purchase.GST = decimal.Parse(GST.Text.Substring(1));
                List<CurrentOrder> orderDetails = new List<CurrentOrder>();

                foreach (GridViewRow row in CurrentOrderGridView.Rows)
                {

                    var partid = row.FindControl("PartID") as Label;
                    var qty = row.FindControl("Qty") as TextBox;
                    var price = row.FindControl("Price") as TextBox;
                    if (int.Parse(qty.Text) < 0)
                    {
                        throw new Exception("Quantity cannot be a negative number");
                    }
                    if (decimal.Parse(price.Text) <= 0)
                    {
                        throw new Exception("Price cannot be a negative number");
                    }
                    if (partid != null)
                    {
                        var details = new CurrentOrder();
                        details.PartID = int.Parse(partid.Text);
                        details.Qty = int.Parse(qty.Text);
                        details.Price = decimal.Parse(price.Text);
                        orderDetails.Add(details);
                    }
                    else
                    {
                        throw new Exception("Empty");
                    }

                }

                var controller = new PurchasingController();
                purchase.OrderDetails = orderDetails;
                controller.UpdateCurrentOrder(purchase);
            }, "Sucess", "Order updated.");

        }




        protected void Delete_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var poid = Convert.ToInt32((CurrentOrderGridView.Rows[0].FindControl("PurchaseOrderID") as HiddenField).Value);
                if (poid == 0)
                {
                    throw new Exception("empty");
                }
                FinalOrder purchase = new FinalOrder();
                purchase.PurchaseOrderID = Convert.ToInt32((CurrentOrderGridView.Rows[0].FindControl("PurchaseOrderID") as HiddenField).Value);
                purchase.VendorID = int.Parse(VendorDropDown.SelectedValue);
                purchase.Subtotal = decimal.Parse(Subtotal.Text.Substring(1));
                purchase.GST = decimal.Parse(GST.Text.Substring(1));
                List<CurrentOrder> orderDetails = new List<CurrentOrder>();

                foreach (GridViewRow row in CurrentOrderGridView.Rows)
                {

                    var partid = row.FindControl("PartID") as Label;
                    var qty = row.FindControl("Qty") as TextBox;
                    var price = row.FindControl("Price") as TextBox;

                    if (partid != null)
                    {
                        var details = new CurrentOrder();
                        details.PartID = int.Parse(partid.Text);
                        details.Qty = int.Parse(qty.Text);
                        details.Price = decimal.Parse(price.Text);
                        orderDetails.Add(details);
                    }
                    else
                    {
                        throw new Exception("Empty");
                    }

                }

                var controller = new PurchasingController();
                controller.DeleteCurrentOrder(purchase);
                OrderDetailsPanel.Enabled = false;
                OrderDetailsPanel.Visible = false;
                VendorDropDown.SelectedIndex = 0;
                VendorName.Text = "";
                Location.Text = "";
                Phone.Text = "";
             }, "Success", "Order deleted");
        }

        private void CalculateTotals()
        {
            decimal subtotal = 0;
            decimal gst = 0;
            decimal total = 0;


            foreach (GridViewRow row in CurrentOrderGridView.Rows)
            {

                var quantity = row.FindControl("Qty") as TextBox;
                var price = row.FindControl("Price") as TextBox;


                subtotal += (decimal.Parse(quantity.Text)) * (decimal.Parse(price.Text));
                gst += ((decimal.Parse(quantity.Text)) * (decimal.Parse(price.Text))) * (decimal)0.05;
                total += ((decimal.Parse(quantity.Text)) * (decimal.Parse(price.Text))) + (((decimal.Parse(quantity.Text)) * (decimal.Parse(price.Text))) * (decimal)0.05);

            }

            Subtotal.Text = subtotal.ToString("C");
            GST.Text = gst.ToString("C");
            Total.Text = total.ToString("C");

        }

       
    }
}