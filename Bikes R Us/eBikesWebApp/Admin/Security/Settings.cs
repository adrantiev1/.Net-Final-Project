using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace eBikesWebApp.Admin.Security
{
    public static class Settings
    {
        public static string AdminRole => ConfigurationManager.AppSettings["adminRole"];
        public static string CustomerRole => ConfigurationManager.AppSettings["customerRole"];
       
        public static string ReceivingRole => ConfigurationManager.AppSettings["receivingRole"];
        public static string ShipperRole => ConfigurationManager.AppSettings["shipperRole"];
        public static string ServicesRole => ConfigurationManager.AppSettings["servicesRole"];
        public static string PurchasingRole => ConfigurationManager.AppSettings["purchasingRole"];
        public static string EmployeeRole => ConfigurationManager.AppSettings["EmployeeRole"];





    }
}