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

using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace EmployeeManagement.Helper
{
    public static class UserSettings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static bool isLogin
        {
            get => AppSettings.GetValueOrDefault(nameof(isLogin), false);
            set => AppSettings.AddOrUpdateValue(nameof(isLogin), value);
        }

        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserName), value);
        }

        public static string Password
        {
            get => AppSettings.GetValueOrDefault(nameof(Password), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        }

        public static void ClearAllData()
        {
            AppSettings.Clear();
        }
    }
}