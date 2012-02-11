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

        private Pivot panarama;
        private bool updatedFromUi;

        // DP for binding index

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

            // If this flag is not checked, the panorama smooth transition is overridden

            Pivot pan = track.panarama;

            var index = (int)change.NewValue;

            if (pan.Items.Count > index)
                pan.SelectedIndex = (int)change.NewValue;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            panarama = AssociatedObject;
            panarama.SelectionChanged += PanaramaSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (panarama != null)
                panarama.SelectionChanged += PanaramaSelectionChanged;
        }

        // Index changed by UI
        private void PanaramaSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updatedFromUi = true;
            SelectedIndex = panarama.SelectedIndex;
        }
    }
}