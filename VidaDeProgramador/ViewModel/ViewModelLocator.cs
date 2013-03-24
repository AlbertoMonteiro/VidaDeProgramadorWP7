using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight;

namespace VidaDeProgramador.ViewModel
{
    public class ViewModelLocator
    {
        private static MainViewModel _main;
        private static TirinhaViewModel _tirinha;

        public ViewModelLocator()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time services and viewmodels
            }
            else
            {
                // Create run time services and view models
            }
            _main = new MainViewModel();
            _tirinha = new TirinhaViewModel();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return _main;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]
        public TirinhaViewModel Tirinha
        {
            get
            {
                return _tirinha;
            }
        }
    }
}