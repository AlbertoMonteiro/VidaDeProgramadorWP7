using VidaDeProgramador.Core;
using VidaDeProgramador.W8.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VidaDeProgramador.W8.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace VidaDeProgramador.W8
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class GroupedItemsPage : VidaDeProgramador.W8.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
            Loaded += GroupedItemsPage_Loaded;
        }

        void GroupedItemsPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModelLocator.Main;
            ViewModelLocator.Main.CarregaTirinhas();
        }
    }
}
