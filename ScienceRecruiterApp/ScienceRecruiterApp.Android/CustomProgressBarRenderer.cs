using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;
using ScienceRecruiterApp;
using ScienceRecruiterApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ScienceRecruiterApp.ViewModel;
using Android.Graphics;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ScienceRecruiterApp.ViewModel.CustomProgressBar), typeof(ScienceRecruiterApp.Droid.CustomProgressBarRenderer))]

namespace ScienceRecruiterApp.Droid
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        private CustomProgressBar _customProgressBar;
        private CustomProgressBarDrawable _customProgressBarDrawable;

        public CustomProgressBarRenderer(Context context) : base(context)
        {
        }

        [System.Obsolete]
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement is CustomProgressBar customProgressBar)
            {
                _customProgressBar = customProgressBar;

                if (Control != null)
                {
                    // Set the color of the progress bar
                    Control.ProgressDrawable.SetColorFilter(customProgressBar.ProgressColor.ToAndroid(), PorterDuff.Mode.SrcIn);

                    // Set the custom progress drawable with a dotted line at the position of the last performance
                    _customProgressBarDrawable = new CustomProgressBarDrawable(Control.ProgressDrawable, customProgressBar.LastPerformance);
                    Control.ProgressDrawable = _customProgressBarDrawable;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (_customProgressBar != null && e.PropertyName == nameof(CustomProgressBar.LastPerformance))
            {
                _customProgressBarDrawable?.UpdateLastPerformance(_customProgressBar.LastPerformance);
            }
        }
    }

    public class CustomProgressBarDrawable : DrawableWrapper
    {
        private readonly Paint _paint;
        private readonly Path _dotLinePath;

        public CustomProgressBarDrawable(Drawable drawable, double lastPerformance) : base(drawable)
        {
            _paint = new Paint(PaintFlags.AntiAlias);
            _paint.SetStyle(Paint.Style.Stroke);
            _paint.Color = Android.Graphics.Color.Red;
            _paint.StrokeWidth = 2.0f;
            _paint.SetPathEffect(new DashPathEffect(new float[] { 4, 2 }, 0));

            var progressMax = 10000;
            var progress = (int)(lastPerformance * progressMax);
            var dotX = progress / (float)progressMax * Bounds.Width();
            _dotLinePath = new Path();
            _dotLinePath.MoveTo(dotX, 0);
            _dotLinePath.LineTo(dotX, Bounds.Bottom);
        }

        public void UpdateLastPerformance(double lastPerformance)
        {
            var progressMax = 10000;
            var progress = (int)(lastPerformance * progressMax);
            var dotX = progress / (float)progressMax * Bounds.Width();
            _dotLinePath.Reset();
            _dotLinePath.MoveTo(dotX, 0);
            _dotLinePath.LineTo(dotX, Bounds.Bottom);
            InvalidateSelf();
        }

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);

            canvas.DrawPath(_dotLinePath, _paint);
        }
    }
}
