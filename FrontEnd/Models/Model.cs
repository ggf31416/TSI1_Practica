using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class Model
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public Boolean isFullTime { get; set; }
        public Int32 Salary { get; set; }
        public double Hourly { get; set; }
        public String StartDate { get; set; }
    }
}