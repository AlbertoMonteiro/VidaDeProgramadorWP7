using AlbertoMonteiroWP7Tools.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace VidaDeProgramador.ViewModel
{
    public class ViewModelLocator
    {
        private readonly NavigationService navigationService;

        public ViewModelLocator()
        {
            navigationService = new NavigationService();
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            
            if (ViewModelBase.IsInDesignModeStatic)
            {
                
            }
            else
            {
                
            }

            SimpleIoc.Default.Register(() => new MainViewModel(navigationService));
            SimpleIoc.Default.Register(() => new TirinhaViewModel(navigationService));
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public TirinhaViewModel Tirinha
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TirinhaViewModel>();
            }
        }
    }
}