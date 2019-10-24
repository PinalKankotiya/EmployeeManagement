using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Views;
using Android.Views.InputMethods;
using Android.Content;
using EmployeeManagement.Activity;
using EmployeeManagement.Helper;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Provider;
using Uri = Android.Net.Uri;
using Java.IO;
using Newtonsoft.Json;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Arch.Lifecycle;
using Android.Icu.Text;
using Android.Media;
using Android;
using Plugin.Media;
using Android.Util;
using System.IO;
using Android.Gms.Location;
using Com.Karumi.Dexter;
using Com.Karumi.Dexter.Listener.Single;
using Com.Karumi.Dexter.Listener;
using Android.Support.V4.App;
using Android.Support.Design.Widget;

namespace EmployeeManagement
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : AppCompatActivity, IPermissionListener
    {
        EditText txtFirstName, txtLastName, txtUserName, txtPassword, txtConfPassword;
        RadioButton rdMale, rdFemale, rdGender;
        Button btnRegister;
        ImageView imgProfile;
        LocationRequest locationRequest;
        FusedLocationProviderClient fusedLocationProviderClient;
        TextView txt_loaction;

        string current_Location, encodedImgString;
        static MainActivity Instance;

        public static MainActivity GetInstance()
        {
            return Instance;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegisterScreen);

            Instance = this;

            //Location setup
            Dexter.WithActivity(this)
                   .WithPermission(Manifest.Permission.AccessFineLocation)
                   .WithListener(this)
                   .Check();

            initialization();
        }

        #region Current Location
        /// <summary>
        /// <description>Set the current Location</description>
        /// </summary>
        /// <param name="text"></param>
        public void currentLocation(string text)
        {
            try
            {
                RunOnUiThread(() =>
                {
                    txt_loaction.Text = text;
                    current_Location = text;
                });
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.fatch_location), ToastLength.Short).Show();
            }
        }
        #endregion

        private void initialization()
        {
            try
            {
                txtFirstName = FindViewById<EditText>(Resource.Id.txtFirstName);
                txtLastName = FindViewById<EditText>(Resource.Id.txtLastName);
                txtUserName = FindViewById<EditText>(Resource.Id.txtUserName);
                txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
                txtConfPassword = FindViewById<EditText>(Resource.Id.txtConfPassword);
                rdMale = FindViewById<RadioButton>(Resource.Id.rgMale);
                rdFemale = FindViewById<RadioButton>(Resource.Id.rgFemale);
                imgProfile = FindViewById<ImageView>(Resource.Id.imgProfile);
                btnRegister = FindViewById<Button>(Resource.Id.btnRegister);
                txt_loaction = FindViewById<TextView>(Resource.Id.txt_location);
                
                rdMale.Click += onRadioButtonClick;
                rdFemale.Click += onRadioButtonClick;
                btnRegister.Click += onRegisterButtonClick;
                imgProfile.Click += onImgProfileClick;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Long).Show();
            }
        }

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
                //imgProfile.SetImageDrawable(null);
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
                if (CurrentFocus != null)
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
                rdGender = (RadioButton)sender;
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
        }
        #endregion

        #region location
        /// <summary>
        /// <description>fatch current location</description>
        /// </summary>
        /// <param name="p0"></param>
        public void OnPermissionGranted(PermissionGrantedResponse p0)
        {
            UpdateLocation();
        }

        public void OnPermissionDenied(PermissionDeniedResponse p0)
        {
            Toast.MakeText(this, GetString(Resource.String.enable_permission), ToastLength.Short).Show();
        }

        public void OnPermissionRationaleShouldBeShown(PermissionRequest p0, IPermissionToken p1)
        { }

        private void UpdateLocation()
        {
            try
            {
                BuildLocationRequest();

                fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
                if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted)
                {
                    return;
                }

                fusedLocationProviderClient.RequestLocationUpdates(locationRequest, GetPendingIntent());
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
        }

        private PendingIntent GetPendingIntent()
        {
            Intent intent = new Intent(this, typeof(MyLocationService));
            try
            {
                intent.SetAction(MyLocationService.ACTION_PROCESS_LOCATION);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Short).Show();
            }
            return PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);
        }

        private void BuildLocationRequest()
        {
            try
            {
                locationRequest = new LocationRequest();
                locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
                locationRequest.SetInterval(5000);
                locationRequest.SetFastestInterval(3000);
                locationRequest.SetSmallestDisplacement(10f);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.fatch_location), ToastLength.Short).Show();
            }
        }
        #endregion

        //Register Buttton Click
        #region Registration
        /// <summary>
        /// <description></description>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onRegisterButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text) || string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtConfPassword.Text) || string.IsNullOrEmpty(encodedImgString))
                {
                    Snackbar.Make(txtUserName, GetString(Resource.String.require_all_Details), Snackbar.LengthLong).Show();
                    //GlobalConst.alertMessageBox(this, "Requir", "Please Enter All the Details..!!");
                }
                else
                {
                    if (txtPassword.Text == txtConfPassword.Text)
                    {
                        RegisterData registerData = new RegisterData()
                        {
                            firstName = txtFirstName.Text.Trim(),
                            lastName = txtLastName.Text.Trim(),
                            userName = txtUserName.Text.Trim(),
                            password = txtPassword.Text.Trim(),
                            gender = rdGender.Text.Trim(),
                            imgUrl = encodedImgString,
                            currentLocation = current_Location
                        };

                        GlobalConst.insert(registerData);
                        Finish();
                    }
                    else
                    {
                        Toast.MakeText(this, GetString(Resource.String.conf_password), ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GetString(Resource.String.somthing_wrong), ToastLength.Long).Show();
            }
        }
        #endregion
    }
}