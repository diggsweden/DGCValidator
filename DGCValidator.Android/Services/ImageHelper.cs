using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;

namespace DGCValidator.Droid.Services
{
    internal static class ImageHelper
    {
        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            var height = (float)options.OutHeight;
            var width = (float)options.OutWidth;
            var inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth)
            {
                inSampleSize = width > height
                    ? height / reqHeight
                    : width / reqWidth;
            }

            return (int)inSampleSize;
        }

        public static Bitmap DecodeSampledBitmapFromResource(Android.Content.Res.Resources res, int resId, int reqWidth, int reqHeight)
        {
            // First decode with inJustDecodeBounds=true to check dimensions
            var options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeResource(res, resId, options);
            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;
            Bitmap optimalSize = BitmapFactory.DecodeResource(res, resId, options);
            return Bitmap.CreateScaledBitmap(optimalSize, reqWidth, reqHeight, false);
        }
    }
}