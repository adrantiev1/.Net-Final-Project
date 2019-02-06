using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBikesData.Entities.POCOs
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OtherName { get; set; }
        public int Position { get; set; }

    }

    public enum UserType { Customer, Employee }
}
