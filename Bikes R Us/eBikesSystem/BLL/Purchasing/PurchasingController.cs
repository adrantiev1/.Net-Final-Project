using eBikesData.Entities;
using eBikesData.Entities.DTOs.Purchasing;
using eBikesData.Entities.POCOs.Purchasing;
using eBikesSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace eBikesSystem.BLL.Purchasing
{
    [DataObject]
    public class PurchasingController
    {
        #region query
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<VendorInfo> ListVendorNames()
        {
            using (var context = new eBikesContext())
            {
                var result = from vendor in context.Vendors
                             select new VendorInfo
                             {
                                 VendorID = vendor.VendorID,
                                 Name = vendor.VendorName,
                                 Location = vendor.City,
                             };
                return result.ToList();

            }
        }
        public VendorInfo GetVendorSummary(int vendorid)
        {
            using (var context = new eBikesContext())
            {
                var result = from vendor in context.Vendors
                             where vendor.VendorID == vendorid
                             select new VendorInfo
                             {
                                 Name = vendor.VendorName,
                                 Location = vendor.City,
                                 Phone = vendor.Phone
                             };
                return result.Single();

            }
        }

        public int getEmployeeID(string username)
        {
            using (var context = new eBikesContext())
            {
                var employeeID = (from person in context.Employees
                                  where (person.FirstName + "." + person.LastName) == username
                                  select person.EmployeeID).FirstOrDefault();
                return employeeID;
            }

        }

        public List<CurrentOrder> GetCurrentOrder(int vendorid, string username)
        {
            using (var context = new eBikesContext())
            {
                if (context.PurchaseOrders.Where(x => x.VendorID == vendorid && x.OrderDate == null).Any())
                {
                    var result = from order in context.PurchaseOrderDetails
                                 where order.PurchaseOrder.VendorID == vendorid
                                 && order.PurchaseOrder.OrderDate == null
                                 select new CurrentOrder
                                 {
                                     PurchaseOrderID = order.PurchaseOrder.PurchaseOrderID,
                                     PurchaseOrderDetailID = order.PurchaseOrderDetailID,
                                     PartID = order.PartID,
                                     Description = order.Part.Description,
                                     QOH = order.Part.QuantityOnHand,
                                     QOO = order.Part.QuantityOnOrder,
                                     ROL = order.Part.ReorderLevel,
                                     Buffer = (order.Part.QuantityOnHand + order.Part.QuantityOnOrder) - order.Part.ReorderLevel,
                                     Qty = order.Quantity,
                                     Price = order.PurchasePrice
                                 };
                    return result.ToList();
                }
                else
                {
                    var purchaseOrder = context.PurchaseOrders.Add(new PurchaseOrder());
                    purchaseOrder.Closed = false;
                    purchaseOrder.VendorID = vendorid;
                    purchaseOrder.Employee = context.Employees.Find(getEmployeeID(username));

                    var result = from part in context.Parts
                                 where part.Vendor.VendorID == vendorid &&
                                 (part.ReorderLevel - (part.QuantityOnHand + part.QuantityOnOrder)) > 0
                                 select new CurrentOrder
                                 {
                                     PurchaseOrderID = purchaseOrder.PurchaseOrderID,
                                     PartID = part.PartID,
                                     Description = part.Description,
                                     QOH = part.QuantityOnHand,
                                     QOO = part.QuantityOnOrder,
                                     ROL = part.ReorderLevel,
                                     Buffer = ((part.QuantityOnHand + part.QuantityOnOrder) - part.ReorderLevel) > 0 ? ((part.QuantityOnHand + part.QuantityOnOrder) - part.ReorderLevel) : 0,
                                     Qty = part.ReorderLevel - part.QuantityOnHand,
                                     Price = part.PurchasePrice
                                 };


                    foreach (var item in result.ToList())
                    {
                        PurchaseOrderDetail detail = new PurchaseOrderDetail();
                        detail.PartID = item.PartID;
                        detail.Quantity = item.Qty;
                        detail.PurchasePrice = item.Price;
                        purchaseOrder.PurchaseOrderDetails.Add(detail);

                        purchaseOrder.SubTotal += (item.Qty * item.Price);
                        purchaseOrder.TaxAmount += ((item.Qty * item.Price) * (decimal)0.05);

                    }

                    context.SaveChanges();
                    return result.ToList();

                }

            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<CurrentOrder> GenInventoryForVendor(int vendorid)
        {
            using (var context = new eBikesContext())
            {

                var result1 = from order in context.PurchaseOrderDetails
                              where order.PurchaseOrder.VendorID == vendorid
                              && order.PurchaseOrder.OrderDate == null
                              select new CurrentOrder
                              {
                                  PurchaseOrderID = order.PurchaseOrder.PurchaseOrderID,
                                  PartID = order.PartID,
                                  Description = order.Part.Description,
                                  QOH = order.Part.QuantityOnHand,
                                  QOO = order.Part.QuantityOnOrder,
                                  ROL = order.Part.ReorderLevel,
                                  Buffer = ((order.Part.QuantityOnHand + order.Part.QuantityOnOrder) - order.Part.ReorderLevel) > 0 ? ((order.Part.QuantityOnHand + order.Part.QuantityOnOrder) - order.Part.ReorderLevel) : 0,
                                  Price = order.Part.PurchasePrice
                              };

                var result2 = from part in context.Parts
                              where part.VendorID == vendorid
                              select new CurrentOrder
                              {
                                  PurchaseOrderID = (from data in result1
                                                     select data.PurchaseOrderID).FirstOrDefault(),
                                  PartID = part.PartID,
                                  Description = part.Description,
                                  QOH = part.QuantityOnHand,
                                  QOO = part.QuantityOnOrder,
                                  ROL = part.ReorderLevel,
                                  Buffer = ((part.QuantityOnHand + part.QuantityOnOrder) - part.ReorderLevel) > 0 ? ((part.QuantityOnHand + part.QuantityOnOrder) - part.ReorderLevel) : 0,
                                  Price = part.PurchasePrice
                              };

                var result3 = result2.Where(p => !result1.Any(p2 => p2.PartID == p.PartID));
                return result3.ToList();

            }
        }


        #endregion

        #region processing
        public void DeleteCurrentOrder(FinalOrder order)
        {
            using (var context = new eBikesContext())
            {

                var updatedOrder = context.PurchaseOrders.Find(order.PurchaseOrderID);
                var details = context.PurchaseOrderDetails.Where(x => x.PurchaseOrderID == updatedOrder.PurchaseOrderID)?.ToList();

                if (updatedOrder == null)
                {
                    throw new Exception("Order does not exist.");
                }
                if (updatedOrder.OrderDate != null)
                {
                    throw new Exception("Order has already been placed. ");
                }
                else
                {

                    //going through the original list of details and comparing it with the new one
                    foreach (var item in details)
                    {
                        context.PurchaseOrderDetails.Remove(item);
                    }

                    context.PurchaseOrders.Remove(updatedOrder);
                    context.SaveChanges();
                }
            }


        }
        public void UpdateCurrentOrder(FinalOrder order)
        {
            using (var context = new eBikesContext())
            {


                var updatedOrder = context.PurchaseOrders.Find(order.PurchaseOrderID);
                var details = context.PurchaseOrderDetails.Where(x => x.PurchaseOrderID == updatedOrder.PurchaseOrderID)?.ToList();

                if (updatedOrder == null)
                {
                    throw new Exception("Order does not exist.");
                }

                else
                {
                    updatedOrder.SubTotal = order.Subtotal;
                    updatedOrder.TaxAmount = order.GST;

                    //going through the original list of details and comparing it with the new one
                    foreach (var item in details)
                    {
                        var changedpart = order.OrderDetails.SingleOrDefault(x => x.PartID == item.PartID);

                        //if part is not there anymore, then delete it
                        if (changedpart == null)
                        {
                            context.Entry(item).State = EntityState.Deleted;
                        }
                        else
                        {
                            item.Quantity = changedpart.Qty;
                            item.PurchasePrice = changedpart.Price;
                            context.Entry(item).State = EntityState.Modified;
                        }
                    }

                    foreach (var item in order.OrderDetails)
                    {
                        var newpart = !details.Any(x => x.PartID == item.PartID);
                        if (newpart == true)
                        {
                            //check if they added a zero
                            if (item.Qty == 0)
                            {
                                throw new Exception("You cannot add a product into an order with 0 as quantity");
                            }
                            else
                            {
                                var newitem = new PurchaseOrderDetail
                                {
                                    PartID = item.PartID,
                                    Quantity = item.Qty,
                                    PurchasePrice = item.Price
                                };
                                updatedOrder.PurchaseOrderDetails.Add(newitem);
                            }

                        }
                    }
                    context.Entry(updatedOrder).State = EntityState.Modified;
                    context.SaveChanges();
                }

            }
        }

        public void PlaceOrder(FinalOrder order)
        {
            using (var context = new eBikesContext())
            {
                var updatedOrder = context.PurchaseOrders.Find(order.PurchaseOrderID);
                var details = context.PurchaseOrderDetails.Where(x => x.PurchaseOrderID == updatedOrder.PurchaseOrderID)?.ToList();

                if (order.OrderDetails.Count == 0)
                {
                    throw new Exception("An empty order cannot be placed. Please, add items to the order.");
                }
                else
                {
                    updatedOrder.OrderDate = DateTime.Today;
                    updatedOrder.PurchaseOrderNumber = context.PurchaseOrders.Max(x => x.PurchaseOrderNumber) + 1;
                    updatedOrder.SubTotal = order.Subtotal;
                    updatedOrder.TaxAmount = order.GST;

                    //going through the original list of details and comparing it with the new one
                    foreach (var item in details)
                    {
                        var changedpart = order.OrderDetails.SingleOrDefault(x => x.PartID == item.PartID);

                        //if part is not there anymore, then delete it
                        if (changedpart == null)
                        {
                            context.Entry(item).State = EntityState.Deleted;
                        }
                        else
                        {
                            item.Quantity = changedpart.Qty;
                            item.PurchasePrice = changedpart.Price;
                            context.Entry(item).State = EntityState.Modified;
                        }
                    }

                    foreach (var item in order.OrderDetails)
                    {
                        var newpart = !details.Any(x => x.PartID == item.PartID);
                        if (newpart == true)
                        {
                            //check if they added a zero
                            if (item.Qty == 0)
                            {
                                throw new Exception("You cannot add a product into an order with 0 as quantity");
                            }
                            else
                            {
                                var newitem = new PurchaseOrderDetail
                                {
                                    PartID = item.PartID,
                                    Quantity = item.Qty,
                                    PurchasePrice = item.Price
                                };
                                updatedOrder.PurchaseOrderDetails.Add(newitem);
                            }

                            //update Quantity on Order after order is placed
                            var updatedPart = context.Parts.Find(item.PartID);
                            updatedPart.QuantityOnOrder = item.Qty;

                        }
                       

                    }
                    context.Entry(updatedOrder).State = EntityState.Modified;
                    context.SaveChanges();
                }

            }

        }
            #endregion
    }
}

