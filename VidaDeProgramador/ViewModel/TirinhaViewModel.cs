using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using VidaDeProgramador.Persistence;
using VidaDeProgramador.WordpressApi;
using NavigationService = AlbertoMonteiroWP7Tools.Navigation.NavigationService;

namespace VidaDeProgramador.ViewModel
{
    public class TirinhaViewModel : ViewModelBase
    {
        private readonly NavigationService navigationService;
        private readonly VDPContext vdpContext;
        private Visibility landscapeLayoutVisible;
        private Visibility portraitLayoutVisible;
        private Tirinha tirinha;
        private int selectedIndex;

        public TirinhaViewModel(NavigationService navigationService)
        {
            this.navigationService = navigationService;
            if (IsInDesignMode)
            {
                Tirinha = new Tirinha
                {
                    Body = @"real historia;
string sender;
sender = ""Walter Mariano"";
Amigo: Cara, você que manja, precisa me dar umas aulas de informática…
Programador: Aulas??
Amigo: É… Tipo, você tem que me ensinar a baixar músicas, instalar coisas… Também nunca consigo imprimir… Não sei nada de informática…
Programador: Ih, não vai dar… Meu diploma é de computação, não de pedagogia…
–
Camiseta: Vá pedir aulas ao Divasca!",
                    Image = "http://vidadeprogramador.com.br/wp-content/uploads/2013/03/tirinha923.png",
                    Link = "http://vidadeprogramador.com.br/2013/03/22/aulas/",
                    Title = "Aulas"
                };
            }
            else
            {
                vdpContext = new VDPContext();
                this.navigationService.Navigated += AtualizaTirinha;
                PropertyChanged+=(sender, args) =>
                {
                    if (args.PropertyName == "SelectedIndex" && SelectedIndex == 2)
                        MessageBox.Show("Carregar comentarios");
                };
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

        public Tirinha Tirinha
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

        private void AtualizaTirinha(object sender, NavigationEventArgs args)
        {
            if (args.Uri.ToString().Contains("TirinhaView.xaml"))
            {
                var tirinha = (Tirinha) IsolatedStorageSettings.ApplicationSettings["TirinhaCorrent"];
                Tirinha = tirinha;
                if (!vdpContext.TirinhasLidas.Any(x => x.Link == tirinha.Link))
                {
                    vdpContext.TirinhasLidas.InsertOnSubmit(new TirinhaLida {Link = tirinha.Link});
                    vdpContext.SubmitChanges();
                }
            }
        }
    }
}