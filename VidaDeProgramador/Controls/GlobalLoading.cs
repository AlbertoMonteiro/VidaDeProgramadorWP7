﻿using System.Collections.Generic;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace VidaDeProgramador.Controls
{
    public class GlobalLoading
    {
        private static GlobalLoading _in;
        private int loadingCount;
        private ProgressIndicator mangoIndicator;
        private readonly Stack<bool> loadingStack;

        private GlobalLoading() { loadingStack = new Stack<bool>(); }

        public static GlobalLoading Instance
        {
            get { return _in ?? (_in = new GlobalLoading()); }
        }

        public void PushLoading()
        {
            if (loadingStack.Count == 0)
            {
                ++loadingCount;
                NotifyValueChanged();
            }
            loadingStack.Push(true);
        }

        public void PopLoading()
        {
            loadingStack.Pop();
            if (loadingStack.Count == 0)
            {
                --loadingCount;
                NotifyValueChanged();
            }
        }

        public void Initialize(PhoneApplicationFrame frame)
        {
            mangoIndicator = new ProgressIndicator();

            frame.Navigated += OnRootFrameNavigated;

            ((PhoneApplicationPage)frame.Content).SetValue(SystemTray.ProgressIndicatorProperty, mangoIndicator);
        }

        private void OnRootFrameNavigated(object sender, NavigationEventArgs e)
        {
            // Use in Mango to share a single progress indicator instance.
            object ee = e.Content;
            var pp = ee as PhoneApplicationPage;
            if (pp != null)
                pp.SetValue(SystemTray.ProgressIndicatorProperty, mangoIndicator);
        }

        private void NotifyValueChanged()
        {
            if (mangoIndicator != null)
            {
                mangoIndicator.IsIndeterminate = loadingCount > 0;

                if (mangoIndicator.IsVisible == false)
                    mangoIndicator.IsVisible = true;

                mangoIndicator.Text = loadingCount > 0 ? "Carregando..." : "";
            }
        }
    }
}