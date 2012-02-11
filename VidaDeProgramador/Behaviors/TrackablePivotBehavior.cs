using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;

namespace VidaDeProgramador.Behaviors
{
    public class TrackablePivotBehavior : Behavior<Pivot>
    {
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(TrackablePivotBehavior), new PropertyMetadata(0, SelectedIndexPropertyChanged));

        private Pivot pivot;

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private static void SelectedIndexPropertyChanged(DependencyObject dpObj, DependencyPropertyChangedEventArgs change)
        {
            if (change.NewValue.GetType() != typeof(int) || dpObj.GetType() != typeof(TrackablePivotBehavior))
                return;

            var track = (TrackablePivotBehavior)dpObj;

            var pivot = track.pivot;

            var index = (int)change.NewValue;

            if (pivot.Items.Count > index)
                pivot.SelectedIndex = (int)change.NewValue;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            pivot = AssociatedObject;
            pivot.SelectionChanged += PivotSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (pivot != null) pivot.SelectionChanged += PivotSelectionChanged;
        }

        private void PivotSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedIndex = pivot.SelectedIndex;
        }
    }
}