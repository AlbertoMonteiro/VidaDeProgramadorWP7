using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using EventTrigger = System.Windows.Interactivity.EventTrigger;
using TriggerAction = System.Windows.Interactivity.TriggerAction;
using TriggerBase = System.Windows.Interactivity.TriggerBase;

namespace VidaDeProgramador.Controls
{
    public class AdvancedListBox : ListBox
    {
        #region Delegates

        public delegate void ListBoxBottomArived();

        #endregion

        private int totalPassado = -1;

        public AdvancedListBox()
        {
            Loaded += (sender, args) =>
            {
                var sv = (ScrollViewer)FindElementRecursive(this, typeof(ScrollViewer));
                if (sv != null)
                    sv.LayoutUpdated += (o, eventArgs) =>
                    {
                        if (Items.Any() && Math.Abs(sv.ScrollableHeight - sv.VerticalOffset) < 0.001)
                        {
                            var totalAtual = Items.Count;
                            var triggers = Interaction.GetTriggers(this).Where(IsBottomArived);
                            foreach (EventToCommand action in triggers.SelectMany(trigger => trigger.Actions).Where(IsEventToCommand))
                            {
                                var total = Math.Max(totalPassado, totalAtual);
                                if (totalPassado < total)
                                {
                                    totalPassado = total;
                                    action.Command.Execute(this);
                                }
                            }
                        }
                    };
            };
        }

        private static bool IsEventToCommand(TriggerAction action)
        {
            return action is EventToCommand;
        }

        private static bool IsBottomArived(TriggerBase t)
        {
            return t is EventTrigger && ((EventTrigger)t).EventName == "OnListBoxBottomArived";
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