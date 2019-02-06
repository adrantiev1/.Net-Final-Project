using eBikesData.Entities.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eBikesWebApp.Admin.Security.POCOs;

namespace eBikesWebApp.Admin.Security
{
    public partial class RegistereBikesUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Secure access to this page
            if (!Request.IsAuthenticated || !User.IsInRole(Settings.AdminRole))
                Response.Redirect("~", true);
        }

        protected void UnregisteredUsersListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if(e.CommandName == "Add")
            {
                var idCtrl = e.Item.FindControl("IdHiddenField") as HiddenField;
                var typeCtrl = e.Item.FindControl("UserTypeLabel") as Label;
                var userNameCtrl = e.Item.FindControl("AssignedUserNameTextBox") as TextBox;
                var emailCtrl = e.Item.FindControl("AssignedEmailTextBox") as TextBox;
                if (idCtrl != null && typeCtrl != null && userNameCtrl != null && emailCtrl != null)
                {
                    MessageUserControl.TryRun(() =>
                    {
                        var controller = new RegistrationController();
                        var user = new UnregisteredUser
                        {
                            Id = int.Parse(idCtrl.Value),
                            AssignedUserName = userNameCtrl.Text,
                            AssignedEmail = emailCtrl.Text,
                            Position = Convert.ToInt32(typeCtrl.Text)
                        };
                        //if (typeCtrl.Text == UserType.Employee.ToString())
                        //    user.UserType = UserType.Employee;
                        //else if (typeCtrl.Text == UserType.Customer.ToString())
                        //    user.UserType = UserType.Customer;
                        //else
                        //    throw new Exception("Unrecognized Northwind User Type. Only exmployees and customers can be added on this page.");

                        controller.RegisterUser(user);
                        UnregisteredUsersListView.DataBind();
                    }, "Added User", $"Successfully added the {typeCtrl.Text} {userNameCtrl.Text}.");
                }
            }
        }
    }
}