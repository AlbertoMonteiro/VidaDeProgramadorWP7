using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace VidaDeProgramador.Behaviors
{
    public class ScrollPositionBehavior : Behavior<ScrollViewer>
    {
        public double Position
        {
            get { return (double)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty = 
            DependencyProperty.Register("Position", typeof(double), typeof(ScrollPositionBehavior), new PropertyMetadata((double)0, OnPositionChanged));
        
        private ScrollViewer scrollViewer;

        protected override void OnAttached()
        {
            base.OnAttached();
            scrollViewer = AssociatedObject;
            scrollViewer.LayoutUpdated += (sender, args) =>
            {
                Position = scrollViewer.VerticalOffset;
            };
        }

        private static void OnPositionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (ScrollPositionBehavior)sender;
            var value = (double)e.NewValue;
            if (Math.Abs(value - 0) < 0.00001)
            {
                behavior.AssociatedObject.ScrollToVerticalOffset(value); 
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
