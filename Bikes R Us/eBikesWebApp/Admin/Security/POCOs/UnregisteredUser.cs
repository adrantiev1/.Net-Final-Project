using eBikesData.Entities.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBikesWebApp.Admin.Security.POCOs
{
    /// <summary>
    /// An UnregisteredUser is one who is a user (Employee, Customer)
    /// without a mapped Website Login account (ApplicationUser)
    /// </summary>
    public class UnregisteredUser
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string OtherName { get; set; }
        public string AssignedUserName { get; set; }
        public string AssignedEmail { get; set; }
    }
}