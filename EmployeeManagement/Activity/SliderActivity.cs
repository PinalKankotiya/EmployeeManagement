using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using EmployeeManagement.Adapter;
using EmployeeManagement.Helper;

namespace EmployeeManagement.Activity
{
    [Activity(Label = "SliderActivity")]
    public class SliderActivity : AppCompatActivity
    {
        ViewPager viewPager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SliderScreen);
            // Create your application here

            initialization();
        }

        private void initialization()
        {
            try
            {
                var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.Title = "Image Slider";

                viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
                var adapter = new SliderImageAdapter(GlobalConst.employeeInfo);
                viewPager.Adapter = adapter;
                viewPager.Adapter.NotifyDataSetChanged();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Long).Show();
            }
        }

        #region Finish activity
        /// <summary>
        /// <description>on back press activity remove from stack</description>
        /// </summary>
        public override void OnBackPressed()
        {
            Finish();
            base.OnBackPressed();
        }
        #endregion
    }
}