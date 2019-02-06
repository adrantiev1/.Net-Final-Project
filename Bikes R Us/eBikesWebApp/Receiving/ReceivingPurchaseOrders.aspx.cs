using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eBikesData.Entities.DTOs.Receiving;
using eBikesData.Entities.POCOs.Receiving;
using eBikesSystem.BLL.ReceivingReturns;
using eBikesWebApp.Admin.Security;

namespace eBikesWebApp.Receiving
{
    public partial class ReceivingPurchaseOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // Secure access to this page
            if (!Request.IsAuthenticated
                || !User.IsInRole(Settings.ReceivingRole)              
              )
                { Response.Redirect("~", true); }
            UserName.Text = User.Identity.Name;
            

        }

        protected void DisplayOrder_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var controller = new ReceivingController();
                OrderPanel.Enabled = true;
                OrderPanel.Visible = true;
                var grvRow = (GridViewRow)((LinkButton)sender).NamingContainer;
                // Populate vendor information labels
                PONumLabel.Text = grvRow.Cells[0].Text;
                Vendor.Text = grvRow.Cells[2].Text;
                VendorPhone.Text = grvRow.Cells[3].Text;
                // Populate GridView through a dataSource
                var orderDetails = controller.GetPurchaseOrderDetails(Convert.ToInt32((grvRow.FindControl("POid") as HiddenField).Value));
                PurchaseOrderDetailsGridView.DataSource = orderDetails;
                PurchaseOrderDetailsGridView.DataBind();
            },"Success","Order found!");
            
            
        }

        protected void ReceiveOrder_Click(object sender, EventArgs e)
        {
            string sccssDetail = "Order Received.";
            MessageUserControl.TryRun(() =>
            {
                try
                {
                    PurchaseOrderProcessed order = new PurchaseOrderProcessed();
                    order.POid = Convert.ToInt32((PurchaseOrderDetailsGridView.Rows[0].FindControl("POid") as HiddenField).Value);
                    List<ReceivedPODetail> detailList = new List<ReceivedPODetail>();
                    foreach (GridViewRow row in PurchaseOrderDetailsGridView.Rows)
                    {
                        // Unpack GridViewRow:
                        var receivingAmount = Convert.ToInt32((row.Cells[4].Controls[1] as TextBox).Text);
                        var qtyOutstanding = Convert.ToInt32((row.Cells[3].Controls[1] as Label).Text);
                        if (receivingAmount > qtyOutstanding)
                            throw new Exception("Quantity receiving cannot be be greater then quantity outstanding.");
                        var returnAmount = Convert.ToInt32((row.Cells[5].Controls[1] as TextBox).Text);
                        var returnReason = (row.Cells[6].Controls[1] as TextBox).Text;
                        if (returnAmount > 0 && returnReason == "")
                            throw new Exception("Must provide reason for each item returned.");
                        var partID = Convert.ToInt32((row.Cells[0].Controls[1] as Label).Text);
                        var poDetailID = Convert.ToInt32((row.FindControl("PODetailID") as HiddenField).Value);
                        var POid = Convert.ToInt32((row.FindControl("POid") as HiddenField).Value);
                      
                        if (receivingAmount > 0 || returnAmount > 0)
                        {
                            // collect the data to be sent to BLL
                            ReceivedPODetail data = new ReceivedPODetail
                            {
                                POid = POid,
                                PODetailID = poDetailID,
                                PartID = partID,
                                QtyReceiving = receivingAmount,
                                QtyReturning = returnAmount,
                                ReturnReason = returnReason
                            };
                            detailList.Add(data);
                        }
                    }
                    // Check what button sent the request and check for item count for processing
                    var button = ((LinkButton)sender).ID;
                    if (detailList.Count == 0 && button == "ReceiveOrder" && UnorderedCart.Items.Count == 0)
                        throw new Exception("No items received in the order.");
                    order.receivedDetails = detailList;
                    var controller = new ReceivingController();
                    bool closed = controller.ReceiveOrder(order);
                    // Refresh GridView and ListView update after receive.
                    var orderDetails = controller.GetPurchaseOrderDetails(order.POid);
                    PurchaseOrderDetailsGridView.DataSource = orderDetails;
                    PurchaseOrderDetailsGridView.DataBind();
                    UnorderedCart.DataBind();
                    // If order is closed refresh hide Order Details and refresh Order Panel.
                    if (closed)
                    {
                        OrderPanel.Visible = false;
                        OrderPanel.Enabled = false;
                        OutstandingOrderGridView.DataBind();
                        sccssDetail = "Order Fulfilled and closed.";
                    }
                }
                catch (FormatException)
                {

                    throw new Exception("Returning and Receiving must be digits. If non returned / received enter 0.");
                }
            },"Success",sccssDetail);
            
            

        }

        protected void UnorderedCart_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            // Push Purchase Order Number into Unordered Cart Items
            e.Values[0] = PONumLabel.Text;



            MessageUserControl.TryRun(() =>
            {
                if (String.IsNullOrWhiteSpace((string)e.Values[1]))
                {
                    e.Cancel = true;
                    throw new Exception("Must provide description.");
                }
                if (String.IsNullOrWhiteSpace((string)e.Values[2]))
                {
                    e.Cancel = true;
                    throw new Exception("Must provide Vendor Part Name.");
                }
                
                if (!int.TryParse((string)e.Values[3],out int qty) || qty < 0)
                {
                    e.Cancel = true;
                    throw new Exception("unordered quantity must be a positive whole number.");
                }

            },"Insert Sucessfull","Unordered part sucesfully added to the return cart");
            

        }

        protected void ForceCloseOrder_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                // Receive any entered quantities
                //ReceiveOrder_Click(sender, e);

                var POid = Convert.ToInt32((PurchaseOrderDetailsGridView.Rows[0].FindControl("POid") as HiddenField).Value);
                var controller = new ReceivingController();
                var closeReason = ForceCloseText.Text;
                if (closeReason == "")
                    throw new Exception("Must provide reason for force close.");

                controller.ForceCloseOrder(POid, closeReason);
                // Hide Order Details panel and refresh Orders GridView.
                OrderPanel.Visible = false;
                OrderPanel.Enabled = false;
                OutstandingOrderGridView.DataBind();

            },"Success","Order was force closed!");
            
        }

        protected void UnorderedCart_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            
        }
    }
}