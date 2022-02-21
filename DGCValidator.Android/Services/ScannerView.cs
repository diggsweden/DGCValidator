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
using Android.Content.PM;
using Android.Graphics;
using Android.Text;
using ZXing.Mobile;
using Xamarin.Essentials;

namespace DGCValidator.Droid.Services
{
    public class ScannerView : View
    {
        int[] SCANNER_ALPHA = { 0, 64, 128, 192, 255, 192, 128, 64 };
        const long ANIMATION_DELAY = 80L;
        const int CURRENT_POINT_OPACITY = 0xA0;
        const int MAX_RESULT_POINTS = 20;
        const int POINT_SIZE = 6;

        Paint paint;
        Bitmap resultBitmap;
        Color maskColor;
        Color resultColor;
        Color frameColor;
        Color laserColor;
        Color textColor;

        int scannerAlpha;
        List<ZXing.ResultPoint> possibleResultPoints;
        private Paint defaultPaint;
        private Paint buttonPaint;
        private Android.Graphics.Bitmap litTorchIcon;
        private Android.Graphics.Bitmap unlitTorchIcon;
        private Android.Graphics.Bitmap cancelIcon;
        private bool hasTorch = false;
        private bool torchOn = false;
        readonly string scanText;
        private bool cancelPressed = false;
        private MobileBarcodeScanner scanner;

        public ScannerView(Context context, string scanText, MobileBarcodeScanner scanner) : base(context)
        {
            this.scanner = scanner;
            this.scanText = scanText ?? "";
            // Initialize these once for performance rather than calling them every time in onDraw().
            paint = new Paint(PaintFlags.AntiAlias);
            maskColor = Color.Gray; // resources.getColor(R.color.viewfinder_mask);
            resultColor = Color.Red; // resources.getColor(R.color.result_view);
            frameColor = Color.Black; // resources.getColor(R.color.viewfinder_frame);
            laserColor = Color.Red; //  resources.getColor(R.color.viewfinder_laser);
            textColor = Color.White;
            scannerAlpha = 0;
            possibleResultPoints = new List<ZXing.ResultPoint>(5);

            SetDisplayValues();
        }

        private void SetDisplayValues()
        {
            //Determine if device has flash/torch
            hasTorch = this.Context.PackageManager.HasSystemFeature(PackageManager.FeatureCameraFlash);
            int iconHeight = 250;
            int iconWidth = iconHeight;
            _ = Resources.DisplayMetrics;
            if (hasTorch)
            {
                //Load torch button icons
                litTorchIcon = ImageHelper.DecodeSampledBitmapFromResource(Resources, Resource.Drawable.flash, iconWidth, iconHeight);
                unlitTorchIcon = ImageHelper.DecodeSampledBitmapFromResource(Resources, Resource.Drawable.flash_off, iconWidth, iconHeight);

            }
            //Load cancel button icons
            cancelIcon = ImageHelper.DecodeSampledBitmapFromResource(Resources, Resource.Drawable.avbryt, iconWidth, iconHeight);
            // Initialize these once for performance rather than calling them every time in onDraw()
            defaultPaint = new Paint(PaintFlags.AntiAlias);
            buttonPaint = new Paint(PaintFlags.AntiAlias);

            SetBackgroundColor(Color.Transparent);
        }

        Rect GetFramingRect(Canvas canvas)
        {
            var width = canvas.Width * 15 / 16;

            var height = canvas.Height * 4 / 10;

            var leftOffset = (canvas.Width - width) / 2;
            var topOffset = (canvas.Height - height) / 2;
            var framingRect = new Rect(leftOffset, topOffset, leftOffset + width, topOffset + height);

            return framingRect;
        }

        public string TopText { get; set; }

        Rect GetCancelButtonRect()
        {
            var metrics = Resources.DisplayMetrics;
            int height;
            int width;
            if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Portrait)
            {

                height = metrics.HeightPixels / 10;
                width = height;
            }
            else
            {
                height = metrics.WidthPixels / 10;
                width = height;
            }

            int leftOffset = metrics.WidthPixels - (width);
            int topOffset = 0;
            var cancelRect = new Rect(leftOffset, topOffset, leftOffset + width, topOffset + height);

            return cancelRect;
        }

        Rect GetCancelIconRect()
        {
            var metrics = Resources.DisplayMetrics;
            int height;
            int width;
            if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Portrait)
            {
                height = metrics.HeightPixels / 30;
                width = height;
            }
            else
            {
                height = metrics.WidthPixels / 30;
                width = height;
            }
            int leftOffset = metrics.WidthPixels - (width + 50);
            int topOffset = 50;
            var cancelRect = new Rect(leftOffset, topOffset, leftOffset + width, topOffset + height);

            return cancelRect;
        }
        Rect GetTorchButtonRect()
        {
            var metrics = Resources.DisplayMetrics;
            int height;
            int width;
            int topOffset;
            if (metrics.HeightPixels >= metrics.WidthPixels)
            {
                height = metrics.HeightPixels / 9;
                width = height;
                topOffset = metrics.HeightPixels / 5 * 4;
            }
            else
            {
                height = metrics.HeightPixels * 2 / 9;
                width = height;
                topOffset = metrics.HeightPixels - (height + 50);
            }
            int leftOffset = metrics.WidthPixels / 2 - width / 2;
            var torchRect = new Rect(leftOffset, topOffset, leftOffset + width, topOffset + height);

            return torchRect;
        }
        //The Rect used to draw the icon
        Rect GetTorchIconRect()
        {
            var metrics = Resources.DisplayMetrics;
            int height;
            int width;
            int topOffset;

            if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Portrait)
            {
                height = metrics.HeightPixels / 12;
                width = height;
                topOffset = metrics.HeightPixels / 5 * 4;
            }
            else
            {
                height = metrics.WidthPixels / 12;
                width = height;
                topOffset = metrics.HeightPixels - (height + 50);
            }

            int leftOffset = metrics.WidthPixels / 2 - width / 2;

            var torchRect = new Rect(leftOffset, topOffset, leftOffset + width, topOffset + height);

            return torchRect;
        }

        protected override void OnDraw(Canvas canvas)
        {

            var scale = this.Context.Resources.DisplayMetrics.Density;

            var frame = GetFramingRect(canvas);
            if (frame == null)
                return;

            var width = canvas.Width;
            var height = canvas.Height;

            //Draw buttons
            defaultPaint.Color = Color.Black;
            defaultPaint.Alpha = 255;
            buttonPaint.Alpha = 0;

            paint.Color = resultBitmap != null ? resultColor : maskColor;
            paint.Alpha = 100;

            canvas.DrawRect(0, 0, width, frame.Top, paint);
            canvas.DrawRect(0, frame.Bottom + 1, width, height, paint);


            var textPaint = new TextPaint();
            textPaint.Color = Color.White;
            textPaint.TextSize = 26 * scale;
            var topTextLayout = new StaticLayout(scanText, textPaint, canvas.Width, Android.Text.Layout.Alignment.AlignCenter, 1.0f, 0.0f, false);
            canvas.Save();
            var topBounds = new Rect();
            textPaint.GetTextBounds(this.scanText, 0, this.scanText.Length, topBounds);
            canvas.Translate(0, frame.Top / 2 - (topTextLayout.Height / 2));

            topTextLayout.Draw(canvas);

            canvas.Restore();

            if (resultBitmap != null)
            {
                paint.Alpha = CURRENT_POINT_OPACITY;
                canvas.DrawBitmap(resultBitmap, null, new RectF(frame.Left, frame.Top, frame.Right, frame.Bottom), paint);
            }
            else
            {
                // Draw a two pixel solid black border inside the framing rect
                paint.Color = frameColor;

                // Draw a red "laser scanner" line through the middle to show decoding is active
                paint.Color = laserColor;
                paint.Alpha = SCANNER_ALPHA[scannerAlpha];
                scannerAlpha = (scannerAlpha + 1) % SCANNER_ALPHA.Length;
                var middle = frame.Height() / 2 + frame.Top;


                canvas.DrawRect(0, middle - 1, width, middle + 2, paint);

                PostInvalidateDelayed(ANIMATION_DELAY,
                                      frame.Left - POINT_SIZE,
                                      frame.Top - POINT_SIZE,
                                      frame.Right + POINT_SIZE,
                                      frame.Bottom + POINT_SIZE);
            }
            if (cancelIcon != null && cancelPressed != true)
            {
                canvas.DrawBitmap(cancelIcon, null, GetCancelIconRect(), null);

                if (hasTorch && litTorchIcon != null && unlitTorchIcon != null)
                {
                    canvas.DrawBitmap(torchOn ? litTorchIcon : unlitTorchIcon, null, GetTorchIconRect(), null);
                }
            }
            base.OnDraw(canvas);

        }

        public void DrawResultBitmap(Android.Graphics.Bitmap barcode)
        {
            resultBitmap = barcode;
            Invalidate();
        }

        public void AddPossibleResultPoint(ZXing.ResultPoint point)
        {
            var points = possibleResultPoints;

            lock (points)
            {
                points.Add(point);
                var size = points.Count;
                if (size > MAX_RESULT_POINTS)
                {
                    points.RemoveRange(0, size - MAX_RESULT_POINTS / 2);
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent me)
        {
            if (me.Action == MotionEventActions.Down)
            {
                if (GetCancelButtonRect().Contains((int)me.RawX, (int)me.RawY))
                {
                    cancelPressed = true;
                    this.Invalidate();
                    OnUnload();
                    scanner.Cancel();

                }
                if (GetTorchButtonRect().Contains((int)me.RawX, (int)me.RawY))
                {
                    scanner.ToggleTorch();
                    torchOn = !torchOn;
                    this.Invalidate();
                }
                return true;
            }
            else
                return false;
        }

        private void OnUnload()
        {
            cancelIcon.Dispose();
            if (hasTorch)
            {
                litTorchIcon.Dispose();
                unlitTorchIcon.Dispose();
            }
        }

    }
}