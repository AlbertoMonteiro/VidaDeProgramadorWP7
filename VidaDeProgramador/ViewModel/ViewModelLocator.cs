using GalaSoft.MvvmLight;

namespace VidaDeProgramador.ViewModel
{
    public class ViewModelLocator
    {
        private static MainViewModel _main;

        public ViewModelLocator()
        {
            _main = new MainViewModel();
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time services and viewmodels
            }
            else
            {
                // Create run time services and view models
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return _main;
            }
        }
    }
}