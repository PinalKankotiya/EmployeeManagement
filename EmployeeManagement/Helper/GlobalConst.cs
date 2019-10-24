using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EmployeeManagement.Models;
using Plugin.Media;

namespace EmployeeManagement.Helper
{
    public static class GlobalConst
    {
        public static List<RegisterData> userInfo = new List<RegisterData>();
        public static List<EmployeeRegisterData> employeeInfo = new List<EmployeeRegisterData>();

        //Add PM Detail
        public static bool insert(RegisterData registerData)
        {
            try
            {
                userInfo.Add(registerData);
            }
            catch (Exception )
            {
               
            }
            return true;
        }

        //Add Employee Details
        public static bool insertEmployee(EmployeeRegisterData empRegisterData)
        {
            try
            {
                employeeInfo.Add(empRegisterData);
            }
            catch (Exception)
            {

            }
            return true;
        }

        //Update Employee Details
        public static bool updateEmployee(EmployeeRegisterData empRegisterData, int Id)
        {
            try
            {
                employeeInfo.RemoveAt(Id);
                employeeInfo.Insert(Id, empRegisterData);
            }
            catch(Exception)
            {

            }
            return true;
        }

        //Delete Employee Details
        public static bool deleteEmployee(int Id)
        {
            try
            {
                employeeInfo.RemoveAt(Id);
            }
            catch (Exception)
            {

            }
            return true;
        }

        //Clear array list
        public static void clear()
        {
            userInfo.Clear();
        }

        public static void alertMessageBox(Context context,string title, string message, Action<bool> clicked)
        {
            Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(context);
            alertDiag.SetTitle(title);
            alertDiag.SetMessage(message);
            alertDiag.SetPositiveButton("ok", (senderAlert, args) =>
            {
                
            });
            clicked(true);
            alertDiag.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                alertDiag.Dispose();
            });
            clicked(false);
            Dialog diag = alertDiag.Create();
            diag.Show();
        }
    }
}