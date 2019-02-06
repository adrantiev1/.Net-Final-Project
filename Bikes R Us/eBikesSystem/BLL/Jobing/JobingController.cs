using eBikesData.Entities;
using eBikesData.Entities.POCOs.Jobbing;
using eBikesSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesSystem.BLL.Jobing
{
    #region Query

    [DataObject]
    public class JobingController
    {
        //customer dropdown
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<POCOCustomer> ListAllCustomers()
        {
            using (var context = new eBikesContext())
            {
                var result = from customer in context.Customers
                             select new POCOCustomer
                             {
                                 CustomerID = customer.CustomerID,
                                 FullName = customer.LastName + " " + customer.FirstName
                             };
                return result.ToList();
            }
        }
       
        //List all job
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<POCOJob> ListAllJobs()
        {
            using (var context = new eBikesContext())
            {

                var result = from job in context.Jobs
                             select new POCOJob
                             {
                                 JobID = job.JobID,
                                 JobDateIn = job.JobDateIn == null ? null : SqlFunctions.DateName("mm", job.JobDateIn).Substring(0, 3) + " " + SqlFunctions.DateName("Day", job.JobDateIn) + ", " + SqlFunctions.DateName("Year", job.JobDateIn),
                                 JobDateStarted = job.JobDateStarted == null ? null : SqlFunctions.DateName("mm", job.JobDateStarted).Substring(0, 3) + " " + SqlFunctions.DateName("Day", job.JobDateStarted) + ", " + SqlFunctions.DateName("Year", job.JobDateStarted),
                                 JobDateDone = job.JobDateDone == null ? null : SqlFunctions.DateName("mm", job.JobDateDone).Substring(0, 3) + " " + SqlFunctions.DateName("Day", job.JobDateDone) + ", " + SqlFunctions.DateName("Year", job.JobDateDone),
                                 CustomerFullName = job.Customer.LastName + " " + job.Customer.FirstName,
                                 Phone = job.Customer.ContactPhone
                             };
                return result.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<POCOCoupon> ListAllCoupon()
        {
            using (var context = new eBikesContext())
            {
                var result = from c in context.Coupons
                             select new POCOCoupon
                             {
                                 CouponID = c.CouponID,
                                 CouponIDValue = c.CouponIDValue
                             };
                return result.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<POCOServiceDetail> ListServiceDetailsByJobID(string jobID)
        {
            int ID = int.Parse(jobID);
            using (var context = new eBikesContext())
            {
                var result = from s in context.ServiceDetails
                             where s.JobID == ID
                             select new POCOServiceDetail
                             {
                                 Description = s.Description,
                                 JobHours = s.JobHours,
                                 Comments = s.Comments,
                                 CouponID = s.CouponID,
                                 ServiceDetailID = s.ServiceDetailID,
                                 CouponIDValue = s.Coupon.CouponIDValue,
                                 Status = s.Status
                             };
                return result.ToList();
            }
        }

        //customer dropdown
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<POCOSDPart> ListAllSDPartsBySDID(int SDID)
        {
            using (var context = new eBikesContext())
            {
                var result = from SDpart in context.ServiceDetailParts
                             where SDpart.ServiceDetailID == SDID
                             select new POCOSDPart
                             {
                                 PartID = SDpart.PartID,
                                 Description = SDpart.Part.Description,
                                 Quantity = SDpart.Quantity,
                                 ServiceDetailID = SDpart.ServiceDetailID,
                                 SellingPrice = SDpart.Part.SellingPrice,
                                 ServiceDetailPartID = SDpart.ServiceDetailPartID
                             };
                return result.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public void UpdateSDPart(POCOSDPart newSDpart)
        {
            using (var context = new eBikesContext())
            {
                // checking part invetory on hand
                var existingSDpart = context.ServiceDetailParts.Find(newSDpart.ServiceDetailPartID);
                var existingSD = context.ServiceDetails.Find(existingSDpart.ServiceDetailID);
                if (existingSD.Status == "C")
                {
                    throw new Exception("Service detail is closed, part can not be updated");
                }
                if (newSDpart.Quantity == 0)
                {
                    throw new Exception("The quantity of part must be a positive whole number");
                }
                var part = context.Parts.Find(newSDpart.PartID);
                if (newSDpart.Quantity > part.QuantityOnHand)
                {
                    throw new Exception($"PartID: {newSDpart.PartID} only has Quantity: {part.QuantityOnHand} in inventory.");
                }
                if (existingSD.Status == "S")
                {
                    var existingPart = context.Parts.Find(newSDpart.PartID);
                    var oldSDpart = context.ServiceDetailParts.Find(newSDpart.ServiceDetailPartID);
                    if (newSDpart.Quantity > oldSDpart.Quantity)
                    {
                        existingPart.QuantityOnHand -= newSDpart.Quantity - oldSDpart.Quantity;
                    }
                    if (newSDpart.Quantity < oldSDpart.Quantity)
                    {
                        existingPart.QuantityOnHand += oldSDpart.Quantity - newSDpart.Quantity;
                    }

                }
                var NewSDpart = context.ServiceDetailParts.Find(newSDpart.ServiceDetailPartID);
                NewSDpart.Quantity = (short)newSDpart.Quantity;
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void RemoveSDPart(POCOSDPart info)
        {
            using (var context = new eBikesContext())
            {
                var existingSDpart = context.ServiceDetailParts.Find(info.ServiceDetailPartID);
                var existingSD = context.ServiceDetails.Find(existingSDpart.ServiceDetailID);

                if (existingSD.Status == "C")
                {
                    throw new Exception("Service detail is closed, part can not be removed");
                }
                // if the serviceDetail already started then return part back to inventory
                if (existingSD.Status == "S")
                {
                    var existingPart = context.Parts.Find(existingSDpart.PartID);

                    existingPart.QuantityOnHand += existingSDpart.Quantity;
                }

                context.ServiceDetailParts.Remove(context.ServiceDetailParts.Find(info.ServiceDetailPartID));
                context.SaveChanges();
            }

        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public void AddNewSDPart(POCOSDPart newSDpart)
        {
            using (var context = new eBikesContext())
            {
                // checking part invetory on hand
                if (newSDpart.ServiceDetailID == 0)
                {
                    throw new Exception("Please select a service detail to add part");
                }
                var existingSD = context.ServiceDetails.Find(newSDpart.ServiceDetailID);
                if (newSDpart.Quantity == 0)
                {
                    throw new Exception("The quantity of part must be a positive whole number");
                }
                if (existingSD.Status == "C")
                {
                    throw new Exception("Selected service detail is closed, can not be edit.");
                }
                var part = context.Parts.Find(newSDpart.PartID);
                if (part == null)
                {
                    throw new Exception("The partID is not valid.");
                }
                if (newSDpart.Quantity > part.QuantityOnHand)
                {
                    throw new Exception($"Failed PartID: {newSDpart.PartID} only has {part.QuantityOnHand} in inventory.");
                }

                if (existingSD.Status == "S")
                {
                    part.QuantityOnHand -= newSDpart.Quantity;
                }

                ServiceDetailPart SDpart = new ServiceDetailPart();
                SDpart.PartID = newSDpart.PartID;
                SDpart.Quantity = (short)newSDpart.Quantity;
                SDpart.ServiceDetailID = newSDpart.ServiceDetailID;
                SDpart.SellingPrice = context.Parts.Find(newSDpart.PartID).SellingPrice;
                context.ServiceDetailParts.Add(SDpart);
                context.SaveChanges();
            }
        }

        #endregion

        #region Processing

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void RemoveServiceDetail(POCOServiceDetail item)
        {
            using (var context = new eBikesContext())
            {
                var selectedItem = context.ServiceDetails.Find(item.ServiceDetailID);

                var SDList = context.ServiceDetails.Where(x => x.JobID == selectedItem.JobID)?.ToList();

                var existingJob = context.Jobs.Find(selectedItem.JobID);
                if(existingJob.StatusCode=="D")
                {
                    throw new Exception("The Job is closed, service details associate with the job cannot be edited or deleted.");
                }

                if (selectedItem.Status == null)
                {
                    //check if the serviceDetail is the last item
                    bool lastItem = false;
                    if (SDList.Count == 0 || SDList.Count == 1)
                    {
                        lastItem = true;
                    }
                    if (lastItem == true)
                    {
                        var job = context.Jobs.Find(selectedItem.JobID);
                        context.Jobs.Remove(job);
                    }
                    context.ServiceDetails.Remove(selectedItem);

                    var SDPartList = context.ServiceDetailParts.Where(x => x.ServiceDetailID == selectedItem.ServiceDetailID).ToList();

                    if (SDPartList.Count != 0)
                    {
                        foreach (var SDpart in SDPartList)
                        {
                            var existingSDpart = context.ServiceDetailParts.Find(SDpart.ServiceDetailPartID);
                            context.ServiceDetailParts.Remove(existingSDpart);
                        }
                    }
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Selected service detail is started or closed, Can not be removed");
                }
            }
        }

        public int getEmployeeID(string username)
        {
            using (var context = new eBikesContext())
            {
                var employee = (from person in context.Employees
                                where (person.FirstName + "." + person.LastName) == username
                                select person.EmployeeID).FirstOrDefault();
                return employee;
            }
        }

        public void AddNewJob(POCOJob newjob, int employeeID, POCOServiceDetail newJobSD)
        {
            using (var context = new eBikesContext())
            {
                Job job = new Job
                {
                    JobDateIn = DateTime.Now,
                    CustomerID = newjob.CustomerID,
                    EmployeeID = employeeID,
                    ShopRate = newjob.ShopRate,
                    StatusCode = "I",
                    VehicleIdentification = newjob.VehicleIdentification
                };
                var newJob = context.Jobs.Add(job);
                //job must be created first
                //context.SaveChanges();                
                //var existingJob = context.Jobs.Where(x => (SqlFunctions.DateName("year", x.JobDateIn)+ SqlFunctions.DateName("month", x.JobDateIn)+ SqlFunctions.DateName("day", x.JobDateIn) == SqlFunctions.DateName("year", DateTime.Today) + SqlFunctions.DateName("month", DateTime.Today)+ SqlFunctions.DateName("day", DateTime.Today)) && x.CustomerID == newjob.CustomerID
                //                                       && x.EmployeeID == employeeID && x.ShopRate == newjob.ShopRate && x.StatusCode == "I"
                //                                       && x.VehicleIdentification == newjob.VehicleIdentification).SingleOrDefault();
                //if (newJob.JobID==0)
                //{
                //    throw new Exception("Failed to create service detail, please add service detail manual ");
                //}
                //else
                //{
                    ServiceDetail newSD = new ServiceDetail
                    {
                        JobID = newJob.JobID,
                        Description = newJobSD.Description,
                        JobHours = newJobSD.JobHours,
                        Comments = newJobSD.Comments,
                        CouponID = newJobSD.CouponID == null ? null : newJobSD.CouponID
                    };
                    context.ServiceDetails.Add(newSD);
                //}
                context.SaveChanges();
            }
        }

        public void AddNewServiceDetail(POCOServiceDetail SD)
        {
            using (var context = new eBikesContext())
            {
                var existingJob = context.Jobs.Find(SD.JobID);
                if(existingJob==null)
                {
                    throw new Exception("The job has been deleted, because there are no service details associate with it.");
                }
                if (existingJob.StatusCode == "D")
                {
                    throw new Exception("The Job is closed, service details associate with the job cannot be edited or deleted.");
                }
                ServiceDetail newSD = new ServiceDetail
                {
                    JobID = SD.JobID,
                    Description = SD.Description,
                    JobHours = SD.JobHours,
                    Comments = SD.Comments,
                    CouponID = SD.CouponID == null ? null : SD.CouponID
                };
                context.ServiceDetails.Add(newSD);
                context.SaveChanges();
            }
        }

        public void UpdateServiceDetailStatusAsS(int pkey)
        {
            using (var context = new eBikesContext())
            {
                var existingSD = context.ServiceDetails.Find(pkey);
                var job = context.Jobs.Find(existingSD.JobID);

                if (existingSD.Status == null)
                {
                    var SDpartList = (from SDpart in context.ServiceDetailParts
                                      where SDpart.ServiceDetailID == pkey
                                      select SDpart).ToList();
                    foreach (var item in SDpartList)
                    {
                        var part = context.Parts.Find(item.PartID);
                        if (item.Quantity <= part.QuantityOnHand)
                        {
                            part.QuantityOnHand -= item.Quantity;

                        }
                        else
                        {
                            throw new Exception($"There are not enough PartID: {item.PartID} in inventory.");
                        }
                    }
                    existingSD.Status = "S";
                    if (job.StatusCode == "I")
                    {
                        job.JobDateStarted = DateTime.Now;
                        job.StatusCode = "S";
                    }
                    context.SaveChanges();
                }
                else if (existingSD.Status == "S")
                {
                    throw new Exception("This Service is in process ");
                }
                else
                {
                    throw new Exception("This Service is closed ");
                }
            }
        }

        public void UpdateServiceDetailStatusAsD(int pkey)
        {
            using (var context = new eBikesContext())
            {
                ServiceDetail existingSD = new ServiceDetail();
                existingSD = context.ServiceDetails.Find(pkey);
                var job = context.Jobs.Find(existingSD.JobID);
                var SDList = (from SD in context.ServiceDetails
                              where SD.JobID == existingSD.JobID
                              select SD.Status).ToList();
                //check if the service detail is the last one to close then change the status on job
                int indicator = 0;
                foreach (var status in SDList)
                {
                    if (status != "C")
                    {
                        indicator++;
                    }
                }
                if (existingSD.Status == "S")
                {
                    existingSD.Status = "C";

                    if (indicator == 1)
                    {
                        job.JobDateDone = DateTime.Now;
                        job.StatusCode = "D";
                    }
                }
                else if (existingSD.Status == null)
                {
                    throw new Exception("This service not start yet");
                }
                else
                {
                    throw new Exception("This service is closed already");
                }
                context.SaveChanges();
            }
        }

        public string UpdateSDComments(int pkey, string comments)
        {
            using (var context = new eBikesContext())
            {
                ServiceDetail newSD = new ServiceDetail();
                newSD = context.ServiceDetails.Find(pkey);
                if(newSD.Status=="C")
                {
                    throw new Exception("The service detail is closed, cannot add more comments.");
                }
                if (newSD.Comments != null)
                {
                    newSD.Comments += " " + comments;
                }
                else
                {
                    newSD.Comments = comments;
                }
                context.SaveChanges();
                return newSD.Comments;
            }
        }
    }
        #endregion
    
}
