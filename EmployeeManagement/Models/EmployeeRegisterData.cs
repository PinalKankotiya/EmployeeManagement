using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EmployeeManagement.Models
{
    public class EmployeeRegisterData
    {
        public int Id { get; set; }
        public string imgUrl { get; set; }
        public string name { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string date { get; set; }
        public string gender { get; set; }
        public string hobbies { get; set; }
        public string designation { get; set; }
    }
}