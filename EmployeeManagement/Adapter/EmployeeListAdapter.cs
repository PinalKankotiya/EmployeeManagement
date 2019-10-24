using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Common.Images;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using EmployeeManagement.Activity;
using EmployeeManagement.Helper;
using EmployeeManagement.Models;

namespace EmployeeManagement.Adapter
{
    class EmployeeListAdapter : BaseAdapter
    {
        private List<EmployeeRegisterData> employeeInfo;
        int n;

        public EmployeeListAdapter(List<EmployeeRegisterData> employeeInfo)
        {
            this.employeeInfo = employeeInfo;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return employeeInfo[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            try
            {
                EmployeeListAdapterViewHolder holder = null;

                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.EmployeeListRow, parent, false);

                var photo = view.FindViewById<ImageView>(Resource.Id.photoImageView);
                var name = view.FindViewById<TextView>(Resource.Id.nameTextView);
                var designation = view.FindViewById<TextView>(Resource.Id.designationTextView);
                var deleteEmployee = view.FindViewById<ImageButton>(Resource.Id.btnDelete);
                var updateEmployee = view.FindViewById<ImageButton>(Resource.Id.btnUpdate);
                //delete.Click += deleteRow();

                view.Tag = new EmployeeListAdapterViewHolder()
                { Photo = photo, Name = name, Designation = designation, DeleteEmployee = deleteEmployee, UpdateEmployee = updateEmployee };

                if (view != null)
                    holder = (EmployeeListAdapterViewHolder)view.Tag;

                byte[] decodedImageString = Base64.Decode(employeeInfo[position].imgUrl, Base64.Default);
                Bitmap imageByte = BitmapFactory.DecodeByteArray(decodedImageString, 0, decodedImageString.Length);
                
                holder.Name.Text = employeeInfo[position].name;
                holder.Designation.Text = employeeInfo[position].designation;
                holder.Photo.SetImageBitmap(imageByte);
                //holder.Delete.Click
                if (!holder.DeleteEmployee.HasOnClickListeners)
                {
                    holder.DeleteEmployee.Click += (sender, e) =>
                    {
                         n = position;
                        GlobalConst.alertMessageBox(view.Context, "Delete Item", "Are You sure You want to delete item?", HandleClicked);
                       
                        NotifyDataSetChanged();
                    };
                }

                if (!holder.UpdateEmployee.HasOnClickListeners)
                {
                    holder.UpdateEmployee.Click += (sender, e) =>
                    {
                    n = position;
                    //Toast.MakeText(view.Context, "Update click" + n, ToastLength.Short).Show();
                    var intent = new Intent(view.Context, typeof(UpateEmployeeProfileActivity));
                    intent.PutExtra("dataPosition", n);
                    view.Context.StartActivity(intent);
                    };
                }
            }
            catch (Exception)
            {
                //Toast.MakeText(view.Context, "", ToastLength.Short).Show();
            }
            return view;
        }

        private void HandleClicked(bool isClick)
        {
            if (isClick)
            {
                GlobalConst.deleteEmployee(n);
            }
            else
            {
                //TODO cancel button click
            }
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return employeeInfo.Count;
            }
        }
    }

    public class EmployeeListAdapterViewHolder : Java.Lang.Object
    {
        public ImageView Photo { get; set; }
        public TextView Name { get; set; }
        public TextView Designation { get; set; }
        public ImageButton DeleteEmployee { get; set; }
        public ImageButton UpdateEmployee { get; set; }
    }
}