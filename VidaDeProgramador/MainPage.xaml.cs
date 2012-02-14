﻿using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace VidaDeProgramador
{
    public partial class MainPage : PhoneApplicationPage
    {
        private double actualHeight;
        private double actualWidth;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnPinchDelta(object sender, PinchGestureEventArgs e)
        {
            var image = (Image) sender;
            if (actualWidth == 0)
                actualWidth = image.Width;
            if (actualHeight == 0)
                actualHeight = image.Height;

            var renderTransform = (CompositeTransform) image.RenderTransform;
            if (e.DistanceRatio > 1)
            {
                var distanceRatio = (int) e.DistanceRatio;
                double d = e.DistanceRatio - distanceRatio;
                renderTransform.ScaleX = Math.Min(renderTransform.ScaleX + d, 2);
                renderTransform.ScaleY = Math.Min(renderTransform.ScaleY + d, 2);
            } else
            {
                renderTransform.ScaleX = Math.Max(renderTransform.ScaleX - (1 - e.DistanceRatio), 1);
                renderTransform.ScaleY = Math.Max(renderTransform.ScaleY - (1 - e.DistanceRatio), 1);
                if (Math.Abs(renderTransform.ScaleX) == 1)
                {
                    image.Width = actualWidth;
                    image.Height = actualHeight;
                }
            }
            image.Width = image.ActualWidth*renderTransform.ScaleX;
            image.Height = image.ActualHeight*renderTransform.ScaleY;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (MainPivot.SelectedIndex != 0)
            {
                e.Cancel = true;
                MainPivot.SelectedIndex = 0;
            }
            base.OnBackKeyPress(e);
        }
    }
}