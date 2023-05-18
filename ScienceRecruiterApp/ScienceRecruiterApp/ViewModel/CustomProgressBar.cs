using System;
using Xamarin.Forms;

namespace ScienceRecruiterApp.ViewModel
{
    public class CustomProgressBar : ProgressBar
    {
        public static readonly BindableProperty LastPerformanceProperty = BindableProperty.Create(
            propertyName: nameof(LastPerformance),
            returnType: typeof(double),
            declaringType: typeof(CustomProgressBar),
            defaultValue: 0.0,
            propertyChanged: OnLastPerformanceChanged);

        public double LastPerformance
        {
            get => (double)GetValue(LastPerformanceProperty);
            set => SetValue(LastPerformanceProperty, value);
        }

        private static void OnLastPerformanceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomProgressBar customProgressBar)
            {
                customProgressBar.InvalidateMeasure();
            }
        }
    }
}


