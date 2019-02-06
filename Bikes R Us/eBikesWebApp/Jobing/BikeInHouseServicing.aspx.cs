using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eBikesData.Entities.POCOs.Jobbing;
using eBikesSystem.BLL.Jobing;
using eBikesWebApp.Admin.Security;

namespace eBikesWebApp.Jobing
{
    public partial class BikeInHouseServicing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Secure access to this page
            if (!Request.IsAuthenticated
                || !User.IsInRole(Settings.ServicesRole)
               )
                Response.Redirect("~", true);
            UserName.Text = User.Identity.Name;
            UserName2.Text = User.Identity.Name;
            UserName3.Text = User.Identity.Name;
            UserNameNewJob.Value = User.Identity.Name;
        }

        protected void CheckForExceptions(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void NewJob_Click(object sender, EventArgs e)
        {
            NewJobPanel.Enabled = true;
            NewJobPanel.Visible = true;
            CurrentJobList.Enabled = false;
            CurrentJobList.Visible = false;
        }

        protected void ViewJob_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                ViewJobPanel.Enabled = true;
                ViewJobPanel.Visible = true;
                CurrentJobList.Enabled = false;
                CurrentJobList.Visible = false;
            }, "Success", "Data found");
        }

        protected void ViewService_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                ViewServicePanel.Enabled = true;
                ViewServicePanel.Visible = true;
                CurrentJobList.Enabled = false;
                CurrentJobList.Visible = false;
            }, "Success", "Data found");
        }

        protected void AddNewJob_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                int customerID = int.Parse(ListAllCustomers.SelectedValue);
                POCOJob newjob = new POCOJob();
              
                if (customerID != 0)
                {
                    newjob.CustomerID = customerID;
                }
                else
                {
                    throw new Exception("Please select a customer");
                }

                try
                {
                    newjob.ShopRate = decimal.Parse(ShopRate.Text);
                }
                catch
                {
                    MessageUserControl.Visible = true;
                    throw new Exception("Shop Rate is required");
                }
                if (newjob.ShopRate <= 0)
                {
                    throw new Exception("Shop Rate must be a positive whole number.");
                }
                if(string.IsNullOrWhiteSpace(VehicleID.Text))
                {
                    throw new Exception("VehicleIdentification is required");
                }
                newjob.VehicleIdentification = VehicleID.Text;

                //First Service Detail
                int index = int.Parse(DropDownListAddJob.SelectedValue);
                POCOServiceDetail SD = new POCOServiceDetail();
                if (string.IsNullOrWhiteSpace(DescriptionAddJob.Text))
                {
                    throw new Exception("Description is required");
                }
                try
                {
                    SD.JobHours = decimal.Parse(HoursAddJob.Text);
                }
                catch
                {
                    throw new Exception("Hours is required");
                }
                if (SD.JobHours <= 0)
                {
                    throw new Exception("Hours must be a positive whole number.");
                }
              
                SD.Description = DescriptionAddJob.Text;

                if (index != 0)
                { SD.CouponID = index; }
                if(!string.IsNullOrWhiteSpace(CommentAddJob.Text))
                {
                    SD.Comments = CommentAddJob.Text;
                }
           
                var controller = new JobingController();
                int employeeID = controller.getEmployeeID(User.Identity.Name);
                controller.AddNewJob(newjob, employeeID, SD);
                MessageUserControl.Visible = true;
                ShowAllJobs.DataBind();
                ListAllCustomers.SelectedIndex = 0;
                ShopRate.Text = "80";
                VehicleID.Text = null;
                DropDownListAddJob.SelectedIndex = 0;
                DescriptionAddJob.Text = null;
                HoursAddJob.Text = null;
                CommentAddJob.Text = null;
            }, "Success", "New job added succesfully");
        }

        protected void Clear1_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                NewJobPanel.Enabled = false;
                NewJobPanel.Visible = false;
                CurrentJobList.Enabled = true;
                CurrentJobList.Visible = true;
                ShowAllJobs.DataBind();
            }, "Success", "Data found");
        }

        protected void ShowAllJobs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                MessageUserControl.TryRun(() =>
                {
                    ViewJobPanel.Enabled = true;
                    ViewJobPanel.Visible = true;
                    CurrentJobList.Enabled = false;
                    CurrentJobList.Visible = false;
                    NewJobPanel.Enabled = false;
                    NewJobPanel.Visible = false;
                    CouponList.SelectedIndex = 0;
                    Description.Text = null;
                    Hours.Text = null;
                    Comment.Text = null;
                    var jobID = e.Item.FindControl("JobIDLabel") as Label;
                    var customerName = e.Item.FindControl("CustomerFullName") as Label;
                    var phone = e.Item.FindControl("Phone") as Label;
                    JobIDLabel2.Text = jobID.Text;
                    CustomerLabel.Text = "Customer: " + customerName.Text;
                    ContactLabel.Text = "Contact: " + phone.Text;
                    ServiceDetails.DataBind();
                }, "Success", "Data found");
            }

            if (e.CommandName == "Serving")
            {
                MessageUserControl.TryRun(() =>
                {
                    ViewServicePanel.Enabled = true;
                    ViewServicePanel.Visible = true;
                    ViewServiceDetail.Enabled = false;
                    ViewServiceDetail.Visible = false;
                    CurrentJobList.Enabled = false;
                    CurrentJobList.Visible = false;
                    NewJobPanel.Enabled = false;
                    NewJobPanel.Visible = false;
                    var jobID = e.Item.FindControl("JobIDLabel") as Label;
                    var customerName = e.Item.FindControl("CustomerFullName") as Label;
                    var phone = e.Item.FindControl("Phone") as Label;
                    JobIDLabel4.Text = jobID.Text;
                    CustomerLabel2.Text = "Customer: " + customerName.Text;
                    ContactLabel2.Text = "Contact: " + phone.Text;
                    ServicesList.DataBind();
                }, "Success", "Data found");
            }
        }

        protected void AddServiceButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {

                int index = int.Parse(CouponList.SelectedValue);
                POCOServiceDetail SD = new POCOServiceDetail();
                SD.JobID = int.Parse(JobIDLabel2.Text);
                try
                {
                    SD.JobHours = decimal.Parse(Hours.Text);
                }
                catch
                {
                    throw new Exception("Hours is required");
                }
                if (SD.JobHours <= 0)
                {
                    throw new Exception("Hours must be a positive whole number.");
                }
                SD.Description = Description.Text;
                if (index != 0)
                { SD.CouponID = index; }

                SD.Comments = Comment.Text;

                var controller = new JobingController();
                controller.AddNewServiceDetail(SD);

                MessageUserControl.Visible = true;
                CouponList.SelectedIndex = 0;
                Description.Text = null;
                Hours.Text = null;
                Comment.Text = null;
                ServiceDetails.DataBind();
            }, "Success", "Add succesfully");
        }


        protected void GoBack_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                ViewJobPanel.Enabled = false;
                ViewJobPanel.Visible = false;

                CurrentJobList.Enabled = true;
                CurrentJobList.Visible = true;
                ShowAllJobs.DataBind();
            }, "Success", "Data Found");
        }

        protected void ServicesList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                MessageUserControl.TryRun(() =>
                {
                    ViewServiceDetail.Enabled = true;
                    ViewServiceDetail.Visible = true;

                    var description = e.Item.FindControl("DescriptionLabel") as Label;
                    var comments = e.Item.FindControl("HiddenFieldComments") as HiddenField;
                    var hours = e.Item.FindControl("HiddenFieldHours") as HiddenField;
                    var SDID = e.Item.FindControl("HiddenFieldSDID") as HiddenField;

                    ViewDescriptionText.Text = description.Text;
                    ViewHoursText.Text = hours.Value;
                    ViewCommentsText.Text = comments.Value;
                    ViewServiceDetailID.Value = SDID.Value;

                }, "Success", "Data Found");
            }

            if (e.CommandName == "Start")
            {
                MessageUserControl.TryRun(() =>
                {
                    var SDID = e.Item.FindControl("HiddenFieldSDID") as HiddenField;
                    int pkey = int.Parse(SDID.Value);
                    var controller = new JobingController();
                    controller.UpdateServiceDetailStatusAsS(pkey);
                    ServicesList.DataBind();
                }, "Success", "The selected Service Detail is started and all parts needed have been taken out from inventory");

            }
            if (e.CommandName == "Done")
            {
                MessageUserControl.TryRun(() =>
                {
                    var SDID = e.Item.FindControl("HiddenFieldSDID") as HiddenField;
                    int pkey = int.Parse(SDID.Value);
                    var controller = new JobingController();
                    controller.UpdateServiceDetailStatusAsD(pkey);
                    ServicesList.DataBind();
                }, "Success", "The selected Service Detail is closed and cannot be edited any more.");

            }
            if (e.CommandName == "Remove")
            {
                MessageUserControl.TryRun(() =>
                {
                    var SDID = e.Item.FindControl("HiddenFieldSDID") as HiddenField;
                    int pkey = int.Parse(SDID.Value);
                    POCOServiceDetail SD = new POCOServiceDetail();
                    SD.ServiceDetailID = pkey;
                    var controller = new JobingController();
                    controller.RemoveServiceDetail(SD);
                    ServicesList.DataBind();
                }, "Success", "The selected Service Detail is Removed. If the last service detail of this job is removed then the job will be deleted. And all service detail parts records will be deleted");
            }
        }

        protected void AddComments_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                int SDID = 0;
                try
                {
                    SDID = int.Parse(ViewServiceDetailID.Value);
                }
                catch
                {
                    throw new Exception("Please select a service to add comments");
                }
                if (!string.IsNullOrWhiteSpace(EditComment.Text))
                {
                    var controller = new JobingController();
                    string newComment = controller.UpdateSDComments(SDID, EditComment.Text);
                    ViewCommentsText.Text = newComment;
                }
                else
                {
                    throw new Exception("Please enter comments");
                }
            }, "Success", "New comments are added.");
        }

        protected void ServiceDetailPartList_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {

                e.Values["ServiceDetailID"] = int.Parse(ViewServiceDetailID.Value);

                if (String.IsNullOrWhiteSpace((string)e.Values["PartID"]))
                {
                    e.Cancel = true;
                    throw new Exception("Must provide partID.");
                }
                if (String.IsNullOrWhiteSpace((string)e.Values["Quantity"]))
                {
                    e.Cancel = true;
                    throw new Exception("Must provide quantity.");
                }
                if (!String.IsNullOrWhiteSpace((string)e.Values["PartID"]) && (!int.TryParse((string)e.Values["PartID"], out int partID) || partID < 0))
                {
                    e.Cancel = true;
                    throw new Exception(" PartID must be a positive whole number.");
                }
                if (!String.IsNullOrWhiteSpace((string)e.Values["Quantity"]) && (!int.TryParse((string)e.Values["Quantity"], out int qty) || qty < 0))
                {
                    e.Cancel = true;
                    throw new Exception(" Quantity must be a positive whole number.");
                }

            }, "Success", "New part added, if the service is started then inventory will be adjusted according to the changes");

        }

        protected void GoBack_Click1(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {

                ViewServiceDetail.Enabled = false;
                ViewServiceDetail.Visible = false;

                ViewServicePanel.Enabled = false;
                ViewServicePanel.Visible = false;
                CurrentJobList.Enabled = true;
                CurrentJobList.Visible = true;
                ShowAllJobs.DataBind();
            }, "Success", "Data found");
        }

        protected void ServiceDetailPartList_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            MessageUserControl.TryRun(() => { }, "Success", "Part deleted. If the service is started then all parts will return back to inventory.");           
        }

        protected void ServiceDetails_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            MessageUserControl.TryRun(() => { }, "Success", "Service detail deleted, and all parts needed are deleted, if the last service detail has been deleted then the job will be deleted as well.");
        }

        protected void ServiceDetailPartList_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            MessageUserControl.TryRun(() => {
                if (String.IsNullOrWhiteSpace((string)e.NewValues["Quantity"]))
                {
                    e.Cancel = true;
                    throw new Exception("Must provide quantity.");
                }
                if (!String.IsNullOrWhiteSpace((string)e.NewValues["Quantity"]) && (!int.TryParse((string)e.NewValues["Quantity"], out int qty) || qty < 0))
                {
                    e.Cancel = true;
                    throw new Exception(" Quantity must be a positive whole number.");
                }

            }, "Success", "Service detail part updated. If the servicedetail is in process then the inventory will be adjusted according to the changes.");
        }
    }
}