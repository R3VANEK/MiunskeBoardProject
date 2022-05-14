using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MiunskeBoardProject.base_models
{
    public class ConnectorControl : Image
    {
        public double Pins
        {
            get { return (double)GetValue(PinsProperty); }
            set { SetValue(PinsProperty, value); }
        }


        public static readonly DependencyProperty PinsProperty =
           DependencyProperty.RegisterAttached(
         "Pins",
         typeof(double),
         typeof(ConnectorControl),
         new FrameworkPropertyMetadata(defaultValue: (double)0,
             flags: FrameworkPropertyMetadataOptions.AffectsRender)
       );


        public static double GetPinsProperty(Image target)
        {
            return (double)target.GetValue(PinsProperty);
        }


        public static void SetPinsProperty(Image target, double value) =>
            target.SetValue(PinsProperty, value);
    }
}
