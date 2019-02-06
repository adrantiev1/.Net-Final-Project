using eBikesSystem.DAL;
using eBikesData.Entities.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesSystem.BLL
{
    public class eBikesUserController
    {
        public List<UserProfile> ListUsers()
        {
            using (var context = new eBikesContext())
            {
                var employees = from emp in context.Employees
                                select new UserProfile
                                {
                                    Id = emp.EmployeeID,
                                    Name = emp.FirstName,
                                    OtherName = emp.LastName,
                                    Position = emp.PositionID
                                };
                
                
                return employees.ToList();
            }
        }
    }
}
