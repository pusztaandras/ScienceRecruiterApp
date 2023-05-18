using CoreGraphics;
using ScienceRecruiterApp.ViewModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ScienceRecruiterApp.ViewModel.CustomProgressBar), typeof(ScienceRecruiterApp.iOS.CustomProgressBarRenderer))]

namespace ScienceRecruiterApp.iOS
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            if (Control is UIProgressView progressView && Element is CustomProgressBar customProgressBar)
            {
                var lastPerformancePosition = (int)(progressView.Bounds.Width * customProgressBar.LastPerformance); // calculate the position of the last performance

                using (var context = UIGraphics.GetCurrentContext())
                {
                    context.SetLineDash(phase: 0, lengths: new System.nfloat[] { 4, 2 }); // set dash pattern for the line
                    context.SetLineWidth(2.0f);
                    context.SetStrokeColor(UIColor.Red.CGColor);
                    context.AddLines(new CGPoint[] {
                        new CGPoint(lastPerformancePosition, 0),
                        new CGPoint(lastPerformancePosition, Bounds.Height)
                    });
                    context.DrawPath(CGPathDrawingMode.Stroke);
                }
            }
        }
    }
}


