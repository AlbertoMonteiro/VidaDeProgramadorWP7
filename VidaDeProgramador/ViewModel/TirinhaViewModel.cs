using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls;
using VidaDeProgramador.Common.Persistence;
using NavigationService = AlbertoMonteiroWP7Tools.Navigation.NavigationService;

namespace VidaDeProgramador.ViewModel
{
    public class TirinhaViewModel : ViewModelBase
    {
        private readonly NavigationService navigationService;
        private readonly VDPContext vdpContext;
        private Visibility landscapeLayoutVisible;
        private Visibility portraitLayoutVisible;
        private TirinhaModel tirinha;
        private int selectedIndex;

        public TirinhaViewModel(NavigationService navigationService)
        {
            this.navigationService = navigationService;
            if (IsInDesignMode)
            {
                Tirinha = new TirinhaModel();
            }
            else
            {
                vdpContext = new VDPContext();
                this.navigationService.Navigated += AtualizaTirinha;
                PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "SelectedIndex" && SelectedIndex == 2)
                        MessageBox.Show("Carregar comentarios");
                };
                OrientacaoAlterada = new RelayCommand<PageOrientation>(pageOrientation =>
                {
                    LandscapeLayoutVisible = pageOrientation == PageOrientation.LandscapeLeft || pageOrientation == PageOrientation.LandscapeRight ? Visibility.Visible :  Visibility.Collapsed;
                    PortraitLayoutVisible = pageOrientation == PageOrientation.PortraitUp ? Visibility.Visible :  Visibility.Collapsed;
                });
            }
        }

        public Visibility LandscapeLayoutVisible
        {
            get { return landscapeLayoutVisible; }
            set
            {
                landscapeLayoutVisible = value;
                RaisePropertyChanged("LandscapeLayoutVisible");
            }
        }

        public Visibility PortraitLayoutVisible
        {
            get { return portraitLayoutVisible; }
            set
            {
                portraitLayoutVisible = value;
                RaisePropertyChanged("PortraitLayoutVisible");
            }
        }

        public TirinhaModel Tirinha
        {
            get { return tirinha; }
            set
            {
                tirinha = value;
                RaisePropertyChanged("Tirinha");
            }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

        public RelayCommand<PageOrientation> OrientacaoAlterada { get; private set; }

        private void AtualizaTirinha(object sender, NavigationEventArgs args)
        {
            if (args.Uri.ToString().Contains("TirinhaView.xaml"))
            {
                PortraitLayoutVisible = Visibility.Visible;
                LandscapeLayoutVisible = Visibility.Collapsed;

                Tirinha = (TirinhaModel)IsolatedStorageSettings.ApplicationSettings["TirinhaCorrent"];
                if (!vdpContext.TirinhasLidas.Any(tirinhaLida => tirinhaLida.Link == Tirinha.Link))
                {
                    vdpContext.TirinhasLidas.InsertOnSubmit(new TirinhaLida { Link = Tirinha.Link });
                    vdpContext.SubmitChanges();
                }
            }
        }
    }
}