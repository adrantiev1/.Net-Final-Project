using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBikesData.Entities;
using eBikesData.Entities.POCOs.Sales;
using eBikesSystem.DAL;

namespace eBikesSystem.BLL.Sales
{
    [DataObject]
    public class SalesController
    {
        #region query
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<CategoryPoco> CategoriesList()
        {
            using (var context = new eBikesContext())
            {
                var results = from c in context.Categories
                              select new CategoryPoco
                              {
                                  CategoryID = c.CategoryID,
                                  CategoryDesc = c.Description,
                                  CategoryCount = c.Parts.Count()

                              };
                return results.ToList();
            }

        }
        public int? PartsCount()
        {
            using (var context = new eBikesContext())
            {
                return context.Parts.Count(); ;
            }

        }
        public List<PartPoco> ListAllPartsByCatId(int categoryId)
        {
            using (var context = new eBikesContext())
            {
                var results = from p in context.Parts
                              where p.Category.CategoryID == categoryId && p.Discontinued == false 
                              orderby p.Description
                              select new PartPoco
                              {
                                  PartID = p.PartID,
                                  PartDesc = p.Description,
                                  SellingPrice = p.SellingPrice,
                                  QtyOnHand = p.QuantityOnHand
                              };
                return results.ToList();
            }

        }
        public List<PartPoco> ListAllParts(int shoppingCartId)
        {
            using (var context = new eBikesContext())
            {
                var results = from p in context.Parts
                              where p.Discontinued == false
                              orderby p.Description 
                              select new PartPoco
                              {
                                  CategoryId = p.CategoryID,
                                  PartID = p.PartID,
                                  PartDesc = p.Description,
                                  SellingPrice = p.SellingPrice,
                                  QtyOnHand = p.QuantityOnHand,
                                  QtyInCart = p.ShoppingCartItems.Where(x => x.ShoppingCartID == shoppingCartId).Select(x => x.Quantity).FirstOrDefault()
                              };
                return results.ToList();
            }
        }
        public List<PartPoco> ListAllParts()
        {
            using (var context = new eBikesContext())
            {
                var results = from p in context.Parts
                              where p.Discontinued == false 
                              orderby p.Description
                              select new PartPoco
                              {
                                  CategoryId = p.CategoryID,
                                  PartID = p.PartID,
                                  PartDesc = p.Description,
                                  SellingPrice = p.SellingPrice,
                                  QtyOnHand = p.QuantityOnHand,
                                  QtyInCart = 0
                              };
                return results.ToList();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ProductInCart> ListProductsinCart(int shoppingCartId)
        {
            using (var context = new eBikesContext())
            {

                var result = from p in context.ShoppingCartItems
                             where p.ShoppingCartID == shoppingCartId
                             //group p by p.Part into ProductByCart
                             select new ProductInCart
                             {
                                 CartItemId = p.ShoppingCartItemID,
                                 PartDesc = p.Part.Description,
                                 Qty = p.Quantity,
                                 SellPrice = p.Part.SellingPrice,
                                 ItemTotalPrice = p.Part.SellingPrice * p.Quantity,
                                 StockAvailabillty = p.Part.QuantityOnHand >= p.Quantity ? "" : "Item is out of stock and is not included in price"
                                 //excludedQty = p.Part.QuantityOnHand >= p.Quantity ? 0 : p.Quantity - p.Part.QuantityOnHand

                             };
                return result.ToList();
            }

        }
        public int RetriveShoppingCartId(string user)
        {
            using (var context = new eBikesContext())
            {
                var existingOnlineCustomerID = Convert.ToInt32((from c in context.OnlineCustomers
                                                                where c.UserName == user
                                                                select c.OnlineCustomerID).SingleOrDefault());
                var existingShoppingCartId = Convert.ToInt32((from s in context.ShoppingCarts
                                                              where s.OnlineCustomer.OnlineCustomerID == existingOnlineCustomerID
                                                              select s.ShoppingCartID).SingleOrDefault());
                return existingShoppingCartId;

            }
        }
        public int CartPartCount()
        {
            using (var context = new eBikesContext())
            {
                return context.ShoppingCartItems.Count();
            }

        }
        public DateTime UpdateDate(int shoppingCartID)
        {
            using (var context = new eBikesContext())
            {
                DateTime date = DateTime.Today;
                date = (DateTime)(context.ShoppingCarts.Where(x => x.ShoppingCartID == shoppingCartID).Select(x => x.UpdatedOn)).SingleOrDefault();
                return date;
            }
        }
        public int CouponValue(string couponIdValue)
        {
            using (var context = new eBikesContext())
            {
                return context.Coupons.Where(x => x.CouponIDValue == couponIdValue).Select(x => x.CouponDiscount).SingleOrDefault();
            }
        }
        public decimal CalculateTotal(int shoppingCartID)
        {
            using (var context = new eBikesContext())
            {

                var itemCountInCart = context.ShoppingCartItems.Count();
                var TotalsList = from p in context.ShoppingCartItems
                                 where p.ShoppingCartID == shoppingCartID
                                 select p.Part.QuantityOnHand >= p.Quantity ? p.Part.SellingPrice * p.Quantity : 0 * p.Part.SellingPrice;

                decimal result = 0;
                foreach (var item in TotalsList)
                {
                    result += Convert.ToDecimal(item);
                }

                return result;
            }
        }

        #endregion

        #region processing
        public void AddToCart(string user, int partId, int qty)
        {
            using (var context = new eBikesContext())
            {



                //check for customer exists
                var existingOnlineCustomerID = Convert.ToInt32((from c in context.OnlineCustomers
                                                                where c.UserName == user
                                                                select c.OnlineCustomerID).SingleOrDefault());

                if (existingOnlineCustomerID > 0)
                {

                    //retrive existing shopingCartID
                    var existingShoppingCartId = Convert.ToInt32((from s in context.ShoppingCarts
                                                                  where s.OnlineCustomer.OnlineCustomerID == existingOnlineCustomerID
                                                                  select s.ShoppingCartID).SingleOrDefault());
                    if (existingShoppingCartId == 0)
                    {
                        var newShoppingCart = context.ShoppingCarts.Add(new ShoppingCart());
                        newShoppingCart.OnlineCustomerID = existingOnlineCustomerID;
                        newShoppingCart.CreatedOn = DateTime.Today;
                        newShoppingCart.UpdatedOn = DateTime.Today;
                    }
                    //retrive existing shopingCartItemID
                    var existingShoppingCarItemtId = Convert.ToInt32((from ci in context.ShoppingCartItems
                                                                      where ci.PartID == partId && ci.ShoppingCartID == existingShoppingCartId
                                                                      select ci.ShoppingCartItemID).SingleOrDefault());


                    if (existingShoppingCarItemtId > 0)
                    {



                        var cartItem = context.ShoppingCartItems.Find(existingShoppingCarItemtId);
                        //update qty for existing part

                        cartItem.Quantity += qty;
                        context.Entry(cartItem).State = EntityState.Modified;
                    }
                    else
                    {
                        //add new shopping cart item
                        var newShoppingCartItem = context.ShoppingCartItems.Add(new ShoppingCartItem());
                        newShoppingCartItem.ShoppingCartID = existingShoppingCartId;
                        newShoppingCartItem.PartID = partId;
                        newShoppingCartItem.Quantity = qty;
                    }



                }
                else
                {
                    //create new online customer
                    var newOnlineCustomer = context.OnlineCustomers.Add(new OnlineCustomer());
                    newOnlineCustomer.UserName = user;
                    newOnlineCustomer.CreatedOn = DateTime.Today;

                    //create shoppingCart
                    var newShoppingCart = context.ShoppingCarts.Add(new ShoppingCart());
                    newShoppingCart.OnlineCustomerID = newOnlineCustomer.OnlineCustomerID;
                    newShoppingCart.CreatedOn = DateTime.Today;
                    newShoppingCart.UpdatedOn = DateTime.Today;

                    //create shopingCartItem
                    var newShoppingCartItem = context.ShoppingCartItems.Add(new ShoppingCartItem());
                    newShoppingCartItem.ShoppingCartID = newShoppingCart.ShoppingCartID;
                    newShoppingCartItem.PartID = partId;
                    newShoppingCartItem.Quantity = qty;
                }


                context.SaveChanges();



            }

        }
        public void DeleteCartItem(int shoppingCarItemtId)
        {
            using (var context = new eBikesContext())
            {
                var cartItem = context.ShoppingCartItems.Find(shoppingCarItemtId);
                context.ShoppingCartItems.Remove(cartItem);
                context.SaveChanges();
            }
        }
        public void UpdateCartItemQty(int shoppingCarItemtId, int qty)
        {
            using (var context = new eBikesContext())
            {
                var cartItem = context.ShoppingCartItems.Find(shoppingCarItemtId);

                cartItem.Quantity = qty;
                context.Entry(cartItem).State = EntityState.Modified;
                context.SaveChanges();
            }

        }
        public void PlaceOrder(SaleSummary finalOrder)
        {
            using (var context = new eBikesContext())
            {
                int? couponId = null;
                Guid guid = Guid.NewGuid();

                if (finalOrder.CouponIdValue != "")
                {
                    couponId = context.Coupons.Where(x => x.CouponIDValue == finalOrder.CouponIdValue).Select(x => x.CouponID).SingleOrDefault();
                }

                //make sale first then sale details
                //create sell
                var newSale = new Sale();


                newSale.SaleDate = DateTime.Today;
                newSale.UserName = finalOrder.UserName;
                newSale.EmployeeID = 1;
                newSale.TaxAmount = finalOrder.TaxAmount;
                newSale.SubTotal = finalOrder.SubTotal;
                newSale.CouponID = couponId;
                newSale.PaymentType = finalOrder.PaymentType;

                foreach (var partDetail in finalOrder.Parts)
                {
                    var newSaleDetail = new SaleDetail();


                    int partId = context.Parts.Where(x => x.Description == partDetail.PartDesc).Select(x => x.PartID).SingleOrDefault();
                    var qtyOnHand = context.Parts.Where(x => x.PartID == partId).Select(x => x.QuantityOnHand).SingleOrDefault();
                    

                    var partToUpdate = context.Parts.Find(partId);

                    // backorder the order
                    if (partDetail.Qty > qtyOnHand)
                    {
                        newSaleDetail.PartID = partId;
                        newSaleDetail.Quantity = partDetail.Qty;
                        newSaleDetail.SellingPrice = partDetail.SellPrice;
                        newSaleDetail.Backordered = true;
                        newSaleDetail.ShippedDate = null;
                    }
                    else
                    {
                        newSaleDetail.PartID = partId;
                        newSaleDetail.Quantity = partDetail.Qty ;
                        newSaleDetail.SellingPrice = partDetail.SellPrice;
                        newSaleDetail.Backordered = false;
                        newSale.PaymentToken = guid;
                        newSaleDetail.ShippedDate = DateTime.Today;

                        partToUpdate.QuantityOnHand = qtyOnHand - partDetail.Qty;
                        //if (qtyOnHand - partDetail.Qty == 0)
                        //{
                        //    partToUpdate.Discontinued = true;
                        //}
                        
                    }
                    //changing stock qty in Parts table
                    var existing = context.Entry(partToUpdate);
                    existing.State = EntityState.Modified;

                    newSale.SaleDetails.Add(newSaleDetail);
                }
                //remove cart itmes and cart
                var cartItemId = finalOrder.Parts.Select(x => x.CartItemId).FirstOrDefault();
                var cartId = context.ShoppingCartItems.Find(cartItemId).ShoppingCartID;

                foreach (var item in finalOrder.Parts)
                {
                    var existing = context.ShoppingCartItems.Find(item.CartItemId);
                    context.ShoppingCartItems.Remove(existing);
                }
                //One save for all changes
                var existingCart = context.ShoppingCarts.Find(cartId);
                context.ShoppingCarts.Remove(existingCart);


                context.Sales.Add(newSale);
                context.SaveChanges();

            }
        }


        #endregion

    }
}