using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using EventTrigger = System.Windows.Interactivity.EventTrigger;

namespace VidaDeProgramador.Controls
{
    public class AdvancedListBox : ListBox
    {
        #region Delegates

        public delegate void ListBoxBottomArived();

        #endregion

        public AdvancedListBox()
        {
            Loaded += (sender, args) =>
            {
                var sv = (ScrollViewer) FindElementRecursive(this, typeof (ScrollViewer));
                if (sv != null)
                    sv.LayoutUpdated += (o, eventArgs) =>
                    {
                        if (Items.Any() && Math.Abs(sv.ScrollableHeight - sv.VerticalOffset) < 0.001)
                            foreach (EventTrigger trigger in Interaction.GetTriggers(this))
                                if (trigger.EventName == "OnListBoxBottomArived")
                                    foreach (EventToCommand action in trigger.Actions.Where(x => x is EventToCommand))
                                        action.Command.Execute(this);
                    };
            };
        }

        public event ListBoxBottomArived OnListBoxBottomArived;

        private UIElement FindElementRecursive(FrameworkElement parent, Type targetType)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            UIElement returnElement = null;
            if (childCount > 0)
                for (int i = 0; i < childCount; i++)
                {
                    Object element = VisualTreeHelper.GetChild(parent, i);
                    if (element.GetType() == targetType)
                        return element as UIElement;
                    returnElement = FindElementRecursive(VisualTreeHelper.GetChild(parent, i) as FrameworkElement, targetType);
                }
            return returnElement;
        }

        private VisualStateGroup FindVisualState(FrameworkElement element, string name)
        {
            if (element == null)
                return null;

            IList groups = VisualStateManager.GetVisualStateGroups(element);
            return groups.Cast<VisualStateGroup>().FirstOrDefault(@group => @group.Name == name);
        }
    }
}