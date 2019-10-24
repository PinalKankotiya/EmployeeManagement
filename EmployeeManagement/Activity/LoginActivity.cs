using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using EmployeeManagement.Activity;
using EmployeeManagement.Helper;

namespace EmployeeManagement
{
    [Activity(Label = "Employee Management", MainLauncher = true, LaunchMode = LaunchMode.SingleTop)]
    public class LoginActivity : AppCompatActivity
    {
        TextView tvForgetPwd;
        EditText txtUserName, txtPasword;
        Button btnLogin, btnSignUp;
        CheckBox checkRemeberMe;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginScreen);

            initialization();

            #region UserSetting
            /// <summary>
            /// <description>Check Stored Login Credential</description>
            /// </summary>
            if (UserSettings.isLogin)
            {
                var intent = new Intent(this, typeof(EmployeeListActivity));
                StartActivity(intent);
                Finish();
            }
            #endregion
        }

        #region Permissions
        /// <summary>
        /// <description>Array of all required permission</description>
        /// </summary>
        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera,
            Manifest.Permission.AccessFineLocation
        };
        #endregion

        #region
        /// <summary>
        /// <description>Check Permission is set or not</description>
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            try
            {
                Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.enable_permission), ToastLength.Short).Show();
            }
        }
        #endregion

        private void initialization()
        {
            try
            {
                //Component Declaration
                txtUserName = FindViewById<EditText>(Resource.Id.txtUserName);
                txtPasword = FindViewById<EditText>(Resource.Id.txtPasword);
                btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
                btnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
                checkRemeberMe = FindViewById<CheckBox>(Resource.Id.checkRemeberMe);
                tvForgetPwd = FindViewById<TextView>(Resource.Id.tvForgetPwd);

                RequestPermissions(permissionGroup, 0);

                btnLogin.Click += onLoginClicked;
                btnSignUp.Click += onSignUpClicked;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
        }

        #region SignUp
        /// <summary>
        /// <description>This function is call when SignUp button click</description>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onSignUpClicked(object sender, EventArgs e)
        {
            try
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                //Finish();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
        }
        #endregion

        #region Dismiss Keyboard
        /// <summary>
        /// <description>Function is used for dismiss keyboard</description>
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            try
            {
                if (this.CurrentFocus != null)
                {
                    InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
                    imm.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.require_all_Details), ToastLength.Short).Show();
            }
            return base.DispatchTouchEvent(ev);
        }
        #endregion


        #region Login

        /// <summary>
        /// <description>Login</description>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onLoginClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPasword.Text))
                {
                    Snackbar.Make(txtUserName, GetString(Resource.String.require_all_Details), Snackbar.LengthLong).Show();
                    //GlobalConst.alertMessageBox(this, "Requir", "Please Enter All the Details..!!",HandleClicked);
                }
                else
                {
                    if (txtUserName.Text.Trim() == "Admin" && txtPasword.Text.Trim() == "admin")
                    {
                        if (checkRemeberMe.Checked)
                        {
                            UserSettings.UserName = txtUserName.Text;
                            UserSettings.Password = txtPasword.Text;
                            UserSettings.isLogin = true;
                        }

                        var intent = new Intent(this, typeof(EmployeeListActivity));
                        StartActivity(intent);
                        Finish();
                        clear();
                    }
                    else
                    {
                        GlobalConst.alertMessageBox(this, "", "Invalid Username and Password", HandleClicked);
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
        }

        private void HandleClicked(bool isClick)
        {
            if (isClick){}
            else{}
        }

        /// <summary>
        /// <description>Clear Variables after button click</description>
        /// </summary>
        private void clear()
        {
            txtUserName.Text = "";
            txtPasword.Text = "";
        }
        #endregion
    }
}