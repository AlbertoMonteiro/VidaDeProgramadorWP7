using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using VidaDeProgramador.Controls;

namespace VidaDeProgramador
{
    public partial class MainPage : PhoneApplicationPage
    {
        private double actualWidth, actualHeight;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnPinchDelta(object sender, PinchGestureEventArgs e)
        {
            var image = (Image)sender;
            if (actualWidth == 0)
                actualWidth = image.Width;
            if (actualHeight == 0)
                actualHeight = image.Height;
            var transform = (CompositeTransform)image.RenderTransform;
            if (e.DistanceRatio > 1)
            {
                var distanceRatio = (int)e.DistanceRatio;
                var d = e.DistanceRatio - distanceRatio;
                transform.ScaleX = Math.Min(transform.ScaleX + d, 2);
                transform.ScaleY = Math.Min(transform.ScaleY + d, 2);
            }
            else
            {

                transform.ScaleX = Math.Max(transform.ScaleX - (1 - e.DistanceRatio), 1);
                transform.ScaleY = Math.Max(transform.ScaleY - (1 - e.DistanceRatio), 1);
                if (Math.Abs(transform.ScaleX) == 1)
                {
                    image.Width = actualWidth;
                    image.Height = actualHeight;
                }
            }
            image.Width = image.ActualWidth * transform.ScaleX;
            image.Height = image.ActualHeight * transform.ScaleY;
        }
    }
}