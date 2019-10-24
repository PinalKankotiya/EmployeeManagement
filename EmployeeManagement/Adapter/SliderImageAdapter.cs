using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using EmployeeManagement.Models;
using Java.Lang;

namespace EmployeeManagement.Adapter
{
    class SliderImageAdapter : PagerAdapter
    {
        ImageView sliderImg;
        private List<EmployeeRegisterData> empImageList;
        View view;

        public SliderImageAdapter(List<EmployeeRegisterData> empImageList)
        {
            this.empImageList = empImageList;
        }

        public override int Count
        {
            get
            {
                return empImageList.Count;
            }
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object objectvalue)
        {
            return view == ((View)objectvalue);
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {

            try
            {
                //SliderImageAdapterViewHolder holder = null;
                view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.SliderImageRow, container, false);
                sliderImg = view.FindViewById<ImageView>(Resource.Id.sliderImg);

                //view.Tag = new SliderImageAdapterViewHolder()
                //{ Photo = sliderImg };

                //if (view != null)
                //    holder = (SliderImageAdapterViewHolder)view.Tag;

                byte[] decodedImageString = Base64.Decode(empImageList[position].imgUrl, Base64.Default);
                Bitmap imageByte = BitmapFactory.DecodeByteArray(decodedImageString, 0, decodedImageString.Length);

                sliderImg.SetImageBitmap(imageByte);


                container.AddView(view);
            }
            catch (System.Exception) { }
            return view;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
        {
            container.RemoveView((View)objectValue);
        }
    }

    //public class SliderImageAdapterViewHolder : Java.Lang.Object
    //{
    //    public ImageView Photo { get; set; }
    //}
}