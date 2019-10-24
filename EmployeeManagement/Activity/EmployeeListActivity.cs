using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using EmployeeManagement.Adapter;
using EmployeeManagement.Helper;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace EmployeeManagement.Activity
{
    [Activity(Label = "EmployeeListActivity", Theme = "@style/Theme.DesignDemo", LaunchMode = LaunchMode.SingleTop)]
    public class EmployeeListActivity : AppCompatActivity
    {
        ListView _employeeList;
        private FloatingActionButton addEmployee;
        DrawerLayout drawerLayout;
        NavigationView navigationView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EmployeeListScreen);

            // Create your application here
            initialization();
            sideMenu();
            setData();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            setData();

        }

        #region Employee list
        /// <summary>
        /// <description>This function is used for create employee list</description>
        /// </summary>
        private void setData()
        {
            try
            {
                var adapter = new EmployeeListAdapter(GlobalConst.employeeInfo);
                _employeeList.Adapter = adapter;
                _employeeList.ItemClick += (sender, e) =>
                {
                    var listItem = GlobalConst.employeeInfo[e.Position].Id;
                };
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Long).Show();
            }
        }
        #endregion

        private void initialization()
        {
            try
            {
                _employeeList = FindViewById<ListView>(Resource.Id.employeeList);
                addEmployee = FindViewById<FloatingActionButton>(Resource.Id.btnAddEmployee);
                addEmployee.Click += addNewEmployee;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Long).Show();
            }
        }

        #region
        /// <summary>
        /// <description>This function ids used for add new employee</description>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addNewEmployee(object sender, EventArgs e)
        {
            try
            {
                //Toast.MakeText(this,"button Clicked",ToastLength.Short).Show();
                var newEmployee = new Intent(this, typeof(EmployeeRegistrationActivity));
                StartActivity(newEmployee);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Long).Show();
            }

        }
        #endregion

        #region Sidemenu
        /// <summary>
        /// <description>this function is used for create side menu</description>
        /// </summary>
        private void sideMenu()
        {
            try
            {
                var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(toolbar);
                SupportActionBar.Title = "Home";
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetDisplayShowTitleEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
                SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

                drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
                navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
                setupDrawerContent(navigationView);
                //var nav_img = navigationView.FindViewById<ImageView>(Resource.Id.userImg);
                //var nav_text = navigationView.FindViewById<TextView>(Resource.Id.navheader_username);
                //nav_text.Text = "welconme";

                var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
                drawerLayout.SetDrawerListener(drawerToggle);
                drawerToggle.SyncState();
                //nav_img.Click += updateProfile;

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Long).Show();
            }
        }
        #endregion

        //private void updateProfile(object sender, EventArgs e)
        //{
        //    Toast.MakeText(this,"clickeddddd",ToastLength.Short).Show();
        //}

        #region
        /// <summary>
        /// <description>this function is used for perform item click of side menu</description>
        /// </summary>
        /// <param name="navigationView"></param>
        void setupDrawerContent(NavigationView navigationView)
        {
            try
            {
                navigationView.NavigationItemSelected += (sender, e) =>
                {
                    switch (e.MenuItem.ItemId)
                    {
                        case Resource.Id.nav_home:
                            {
                                Toast.MakeText(this, "Home", ToastLength.Short).Show();
                                //var NAVHome = new Intent(this, typeof(EmployeeListActivity)).SetFlags(ActivityFlags.ClearTask);
                                //StartActivity(NAVHome);
                                //Finish();
                                break;
                            }

                        case Resource.Id.nav_slider:
                            {
                                var slider = new Intent(this, typeof(SliderActivity)).SetFlags(ActivityFlags.ClearTask);
                                StartActivity(slider);
                                //Finish();
                                break;
                            }

                        case Resource.Id.nav_logout:
                            {
                                // UserSettings.ClearAllData();
                                UserSettings.isLogin = false;
                                var intent = new Intent(this, typeof(LoginActivity));
                                StartActivity(intent);
                                Finish();
                                break;
                            }

                        default:
                            break;
                    }
                    // e.MenuItem.SetChecked(true);
                    //e.MenuItem.SetChecked(false);
                    drawerLayout.CloseDrawers();
                };
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error " + ex, ToastLength.Short).Show();
            }
        }
        #endregion

        #region menu option
        /// <summary>
        /// <description>Navigation Drawer Layout Menu Creation </description>
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            try
            {
                navigationView.InflateMenu(Resource.Menu.menu);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
            return base.OnCreateOptionsMenu(menu); 
        }
        #endregion

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            try
            {
                //switch (item.ItemId)
                //{
                //    case Resource.Id.nav_home:
                //        {
                //            Toast.MakeText(this, "Home", ToastLength.Short).Show();
                //            //var NAVHome = new Intent(this, typeof(EmployeeListActivity)).SetFlags(ActivityFlags.ClearTask);
                //            //StartActivity(NAVHome);
                //            //Finish();
                //            break;
                //        }

                //    case Resource.Id.nav_slider:
                //        {
                //            var slider = new Intent(this, typeof(SliderActivity)).SetFlags(ActivityFlags.ClearTask);
                //            StartActivity(slider);
                //            //Finish();
                //            break;
                //        }

                //    case Resource.Id.nav_logout:
                //        {
                //            // UserSettings.ClearAllData();
                //            UserSettings.isLogin = false;
                //            var intent = new Intent(this, typeof(LoginActivity));
                //            StartActivity(intent);
                //            Finish();
                //            break;
                //        }

                //    default:
                //        break;
                //}
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error " + ex, ToastLength.Short).Show();
            }
            return base.OnOptionsItemSelected(item);
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