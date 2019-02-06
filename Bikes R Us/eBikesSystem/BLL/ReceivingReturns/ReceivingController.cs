using eBikesData.Entities;
using eBikesData.Entities.DTOs.Receiving;
using eBikesData.Entities.POCOs.Receiving;
using eBikesSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesSystem.BLL.ReceivingReturns
{
    [DataObject]
    public class ReceivingController
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<OutsandingOrder> ListOutstandingOrder()
        {
            using (var context = new eBikesContext())
            {
                var result = from po in context.PurchaseOrders
                             where !po.Closed && po.OrderDate.HasValue && po.PurchaseOrderNumber.HasValue
                             select new OutsandingOrder
                             {
                                 VendorId = po.VendorID,
                                 VendorName = po.Vendor.VendorName,
                                 VendorPhone = po.Vendor.Phone,
                                 POid = po.PurchaseOrderID,
                                 PONumber = po.PurchaseOrderNumber.Value,
                                 OrderDate = po.OrderDate.Value
                             };

                return result.ToList();

            }
            
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<PODetail> GetPurchaseOrderDetails (int purchaseOrderId)
        {
            using (var context = new eBikesContext())
            {
                var result = from p in context.PurchaseOrderDetails
                             where p.PurchaseOrderID == purchaseOrderId

                             select new PODetail
                             {
                                 POid = p.PurchaseOrderID,
                                 PODetailID = p.PurchaseOrderDetailID,
                                 PartID = p.PartID,
                                 Description = p.Part.Description,
                                 QtyOrdered = p.Quantity,
                                 QtyOutstanding = p.Quantity - (p.ReceiveOrderDetails.FirstOrDefault() == null ? 0 : p.ReceiveOrderDetails.Sum(x=> x.QuantityReceived))
                             };
                return result.ToList();
            }
        }

        public bool ReceiveOrder(PurchaseOrderProcessed order)
        {
            using (var context = new eBikesContext())
            {
                bool fulfilled = false;
                //  Create and populate Receive Order
                var receiveOrder = context.ReceiveOrders.Add( new ReceiveOrder());
                receiveOrder.PurchaseOrderID = order.POid;
                receiveOrder.ReceiveDate = DateTime.Now;
                
                foreach (var item in order.receivedDetails)
                {
                    fulfilled = true;
                    // Update Part QOH and QOO
                    var part = context.Parts.Find(item.PartID);
                    part.QuantityOnHand += item.QtyReceiving;
                    part.QuantityOnOrder -= item.QtyReceiving;
                    context.Entry(part).State = EntityState.Modified; // FLAG for UPDATE
                    // Create ReceiveOrderDetail and populate
                    if (item.QtyReceiving > 0)
                    {
                        var newReceiveDetail = new ReceiveOrderDetail
                        {
                            PurchaseOrderDetailID = item.PODetailID,
                            QuantityReceived = item.QtyReceiving
                        };
                        receiveOrder.ReceiveOrderDetails.Add(newReceiveDetail); // FLAG new ReceiveOrderDetail
                    }
                    // Create ReturnOrderDetail and populate
                    if (item.QtyReturning > 0)
                    {
                        var newReturnDetail = new ReturnedOrderDetail
                        {
                            PurchaseOrderDetailID = item.PODetailID,
                            Quantity = item.QtyReturning,
                            Reason = item.ReturnReason
                        };
                        receiveOrder.ReturnedOrderDetails.Add(newReturnDetail); // FLAG new ReturnOrderDetail
                    }
                    var pOrderDetail = context.PurchaseOrderDetails.Find(item.PODetailID);
                    var qtyOutstaning = pOrderDetail.Quantity - (pOrderDetail.ReceiveOrderDetails.FirstOrDefault() == null ? 0 : pOrderDetail.ReceiveOrderDetails.Sum(x => x.QuantityReceived));
                    if (qtyOutstaning != 0)
                        fulfilled = false; 
                }
                // close order if fulfilled comes back true
                var pOrder = context.PurchaseOrders.Find(order.POid);
                if (fulfilled)
                {
                    
                    pOrder.Closed = fulfilled;
                    context.Entry(pOrder).State = EntityState.Modified; // FLAG updated PurchaseOrder
                }
                // Move data from Unordered Cart into returned items
                List<UnorderedPart> partList = GetUnorderedPart(pOrder.PurchaseOrderNumber.Value);
                foreach (var item in partList)
                {
                    var newReturnDetail = new ReturnedOrderDetail
                    {
                        Quantity = item.Quantity,
                        Reason = "Not Part of Order",
                        ItemDescription = item.Description,
                        VendorPartNumber = item.VendorPartNumber
                    };
                    receiveOrder.ReturnedOrderDetails.Add(newReturnDetail);
                }
                
                // Empty the Unordered Purchase Cart
                var unorderedParts = from u in context.UnorderedPurchaseItemCarts
                                     where u.PurchaseOrderNumber == pOrder.PurchaseOrderNumber
                                     select u;
                var unorderedPartsList = unorderedParts.ToList();

                foreach (var item in unorderedPartsList)
                {
                    context.UnorderedPurchaseItemCarts.Remove(item); // FLAG delete Cart items
                }               
                context.SaveChanges(); // PROCCESS ALL CHANGES
                return fulfilled;
            }           
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<UnorderedPart> GetUnorderedPart(int purchaseOrderNum)
        {
            using (var context = new eBikesContext())
            {
                var unorderedCart = from unordered in context.UnorderedPurchaseItemCarts
                                    where unordered.PurchaseOrderNumber == purchaseOrderNum
                                    select new UnorderedPart
                                    {
                                        CartID = unordered.CartID,
                                        PONumber = unordered.PurchaseOrderNumber,
                                        Description = unordered.Description,
                                        Quantity = unordered.Quantity,
                                        VendorPartNumber = unordered.VendorPartNumber
                                    };

                return unorderedCart.ToList();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public void AddUnorderedItem(UnorderedPart item)
        {
            using (var context = new eBikesContext())
            {
                var unorderedPart = new UnorderedPurchaseItemCart()
                {
                    PurchaseOrderNumber = item.PONumber,
                    Description = item.Description,
                    Quantity = item.Quantity,
                    VendorPartNumber = item.VendorPartNumber
                };
                context.UnorderedPurchaseItemCarts.Add(unorderedPart);
                context.SaveChanges();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void RemoveUnorderedItem(UnorderedPart item)
        {
            using (var context = new eBikesContext())
            {
                var existingItem = context.UnorderedPurchaseItemCarts.Find(item.CartID);
                context.UnorderedPurchaseItemCarts.Remove(existingItem);
                context.SaveChanges();
            }
                
        }
        public void ForceCloseOrder(int POid, string reason)
        {
            using (var context = new eBikesContext())
            {
                
                var existingPOrder = context.PurchaseOrders.Find(POid);
                var result = from detail in context.PurchaseOrderDetails
                             where detail.PurchaseOrderID == POid
                             select detail;
                foreach (var detail in result.ToList())
                {

                    //var qtyOutstaning = detail.Quantity - (detail.ReceiveOrderDetails.FirstOrDefault() == null ? 0 : detail.ReceiveOrderDetails.Sum(x => x.QuantityReceived));
                    //detail.Part.QuantityOnOrder -= qtyOutstaning;
                    int qtyOutstanding;
                    int qtyReceived;
                    var tempReceiveDetails = from receiveDetail in context.ReceiveOrderDetails
                                              where receiveDetail.PurchaseOrderDetailID == detail.PurchaseOrderDetailID
                                              select receiveDetail;
                    if (tempReceiveDetails != null && tempReceiveDetails.ToList().Count > 0)
                        qtyReceived = tempReceiveDetails.ToList().Sum(x => x.QuantityReceived);
                    else
                        qtyReceived = 0;
                    qtyOutstanding = detail.Quantity - qtyReceived;
                    
                    var currentPart = context.Parts.Find(detail.PartID);
                    currentPart.QuantityOnOrder -= qtyOutstanding;
                    var existingItem = context.Entry(currentPart);
                    existingItem.State = EntityState.Modified;
                    
                }
                existingPOrder.Notes = reason;
                existingPOrder.Closed = true;
                var existing = context.Entry(existingPOrder);
                existing.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

    }
}
