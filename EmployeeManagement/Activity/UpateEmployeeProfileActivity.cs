using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using EmployeeManagement.Helper;
using EmployeeManagement.Models;
using Plugin.Media;

namespace EmployeeManagement.Activity
{
    [Activity(Label = "UpateEmployeeProfileActivity", Theme = "@style/Theme.DesignDemo", LaunchMode = LaunchMode.SingleTop)]
    public class UpateEmployeeProfileActivity : AppCompatActivity
    {

        EditText txtName, txtUserName, txtPassword, txtDate;
        RadioButton rgMale, rgFemale, rdGender;
        CheckBox checkSinging, checkDancing, checkOutdoorGame, checkIndoorGame;
        Button btnUpdate;
        ImageView imgProfile;
        Spinner designationList;
        string checkboxText, selectedDesignation, encodedImgString = "";
        int id_loction;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EmployeeRegisterScreen);
            // Create your application here
            
            initialization();
            setData();
            id_loction = Intent.GetIntExtra("dataPosition", -1);
            //_txtName.Text = ""+id_loction;
            //Toast.MakeText(this, ""+id_loction, ToastLength.Short).Show();
           
        }

        private void initialization()
        {
            try
            {
                txtName = FindViewById<EditText>(Resource.Id.txtName);
                txtUserName = FindViewById<EditText>(Resource.Id.txtUserName);
                txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
                txtDate = FindViewById<EditText>(Resource.Id.txtDate);
                rgMale = FindViewById<RadioButton>(Resource.Id.rgMale);
                rgFemale = FindViewById<RadioButton>(Resource.Id.rgFemale);
                checkSinging = FindViewById<CheckBox>(Resource.Id.checkSinging);
                checkDancing = FindViewById<CheckBox>(Resource.Id.checkDancing);
                checkOutdoorGame = FindViewById<CheckBox>(Resource.Id.checkOutdoorGame);
                checkIndoorGame = FindViewById<CheckBox>(Resource.Id.checkIndoorGame);
                btnUpdate = FindViewById<Button>(Resource.Id.btnRegister);
                imgProfile = FindViewById<ImageView>(Resource.Id.imgProfile);
                designationList = FindViewById<Spinner>(Resource.Id.designationList);
                
                rgMale.Click += onRadioButtonClick;
                rgFemale.Click += onRadioButtonClick;
                imgProfile.Click += onImgProfileClick;
                txtDate.Click += DateSelect_OnClick;
                btnUpdate.Click += onUpdateClick;
                checkSinging.Click += onCheckBoxClick;
                checkDancing.Click += onCheckBoxClick;
                checkOutdoorGame.Click += onCheckBoxClick;
                checkIndoorGame.Click += onCheckBoxClick;
                designationList.ItemSelected += designationItemSelected;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
        }

        #region set employee data
        /// <summary>
        /// <description>Set Employee data for update</description>
        /// </summary>
        private void setData()
        {
            try
            {
                //GlobalConst.employeeInfo();
                var v = GlobalConst.employeeInfo[id_loction];
                txtName.Text = v.name;
                txtUserName.Text = v.userName;
                txtPassword.Text = v.password;
                selectedDesignation = v.designation;
                encodedImgString = v.imgUrl;
                byte[] decodedImageString = Base64.Decode(v.imgUrl, Base64.Default);
                Bitmap imageByte = BitmapFactory.DecodeByteArray(decodedImageString, 0, decodedImageString.Length);
                imgProfile.SetImageBitmap(imageByte);

                txtDate.Text = v.date;
                selectedDesignation = v.designation;
                decodedImageString = null;
                imageByte.Dispose();
            }
            catch(Exception)
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
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
            return base.DispatchTouchEvent(ev);
        }
        #endregion

        #region Upload Photo
        /// <summary>
        /// <description>Upload User Profile Picture</description>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onImgProfileClick(object sender, EventArgs e)
        {
            try
            {
                string[] items = { "Take Photo", "Choose from Library", "Cancel" };
                Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(this);
                alertDiag.SetTitle("Add Photo");
                alertDiag.SetItems(items, (d, args) =>
                {
                    //Take photo
                    if (args.Which == 0)
                    {
                        TakePhoto();
                    }

                    else if (args.Which == 1)
                    {
                        UploadPhoto();
                    }
                    else
                    {
                        alertDiag.Dispose();
                    }
                });
                Dialog diag = alertDiag.Create();
                alertDiag.Show();

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
        }

        /// <summary>
        /// <description>Upload photo using camera</description>
        /// </summary>
        async void TakePhoto()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    CompressionQuality = 40,
                    Name = "myimage.jpeg",
                    Directory = "sample"
                });

                if (file == null)
                {
                    return;
                }

                byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
                encodedImgString = Base64.EncodeToString(imageArray, Base64.Default);

                Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
                imgProfile.SetImageBitmap(bitmap);
                imageArray = null;
                bitmap.Dispose();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.image_not_selected), ToastLength.Short).Show();
            }
        }

        /// <summary>
        /// <description>upload photo using Gallery</description>
        /// </summary>
        async void UploadPhoto()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    Toast.MakeText(this, GetString(Resource.String.upload_fail), ToastLength.Short).Show();
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                    CompressionQuality = 40
                });

                byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
                encodedImgString = Base64.EncodeToString(imageArray, Base64.Default);
                Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
                imgProfile.SetImageBitmap(bitmap);
                imageArray = null;
                bitmap.Dispose();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.image_not_selected), ToastLength.Short).Show();
            }
        }
        #endregion

        #region Gender
        /// <summary>
        /// <description>fatch selected value</description>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void onRadioButtonClick(object sender, EventArgs e)
        {
            try
            {
                if(GlobalConst.employeeInfo[id_loction].gender.Equals("Female"))
                rdGender.Checked = true;

                if (GlobalConst.employeeInfo[id_loction].gender.Equals("Male"))
                    rdGender.Checked = true;
                // Toast.MakeText(this, "Selected" + rdGender.Text, ToastLength.Short).Show();

                rdGender = (RadioButton)sender;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.image_not_selected), ToastLength.Short).Show();
            }
        }
        #endregion

        #region Date selection
        /// <summary>
        /// <description>function is used for select date of birth</description>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                //DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                //{
                //    _txtDate.Text = time.ToLongDateString();
                //});
                //frag.Show(FragmentManager, DatePickerFragment.TAG);

                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(this, OnDateSet, today.Year, today.Month - 1, today.Day);
                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error " + ex, ToastLength.Short).Show();
            }
        }

        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            txtDate.Text = e.Date.ToShortDateString();
        }
        #endregion

        #region Hobbies
        /// <summary>
        /// <description>fatch seleted value</description>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onCheckBoxClick(object sender, EventArgs e)
        {
            try
            {
                var checkboxList = new List<CheckBox>();
                checkboxList.Add(checkSinging);
                checkboxList.Add(checkDancing);
                checkboxList.Add(checkOutdoorGame);
                checkboxList.Add(checkIndoorGame);
                checkboxText = "";

                foreach (var checkbox in checkboxList)
                {
                    if (checkbox.Checked == true)
                    {
                        checkboxText += checkbox.Text + ",";
                    }
                    else
                    {

                    }
                }
                //Toast.MakeText(this, checkboxText, ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
        }
        #endregion

        #region Designation
        /// <summary>
        /// <description>Function is used for select designation</description>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void designationItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                //var spinner = (Spinner)sender;
                selectedDesignation = e.Parent.GetItemAtPosition(e.Position).ToString();
                //Toast.MakeText(this, selectedDesignation, ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
        }
        #endregion

        #region Update Profile
        /// <summary>
        /// <description>Update Employee Profile</description>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onUpdateClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtDate.Text) || string.IsNullOrEmpty(checkboxText) || string.IsNullOrEmpty(selectedDesignation) || string.IsNullOrEmpty(encodedImgString))
                {
                    Snackbar.Make(txtUserName, GetString(Resource.String.require_all_Details), Snackbar.LengthLong).Show();
                    //GlobalConst.alertMessageBox(this, "Requir", "Please Enter All the Details..!!");
                }
                else
                {
                    EmployeeRegisterData employeeRegisterData = new EmployeeRegisterData()
                    {
                        imgUrl = encodedImgString,
                        name = txtName.Text,
                        userName = txtUserName.Text,
                        password = txtPassword.Text,
                        date = txtDate.Text,
                        gender = rdGender.Text,
                        hobbies = checkboxText,
                        designation = selectedDesignation
                    };

                    GlobalConst.updateEmployee(employeeRegisterData,id_loction);
                    // var intent = new Intent(this, typeof(EmployeeListActivity));
                    //StartActivity(intent);
                    Finish();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error " + ex, ToastLength.Short).Show();
            }
        }
        #endregion
    }
}