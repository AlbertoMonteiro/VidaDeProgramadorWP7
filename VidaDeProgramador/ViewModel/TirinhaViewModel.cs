using System.Windows;
using GalaSoft.MvvmLight;
using VidaDeProgramador.WordpressApi;

namespace VidaDeProgramador.ViewModel
{
    public class TirinhaViewModel : ViewModelBase
    {
        private Visibility _landscapeLayoutVisible;
        private Visibility _portraitLayoutVisible;
        private Tirinha _tirinha;

        public TirinhaViewModel()
        {
            if (IsInDesignMode || IsInDesignModeStatic)
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
        }

        public Visibility LandscapeLayoutVisible
        {
            get { return _landscapeLayoutVisible; }
            set
            {
                _landscapeLayoutVisible = value;
                RaisePropertyChanged("LandscapeLayoutVisible");
            }
        }

        public Visibility PortraitLayoutVisible
        {
            get { return _portraitLayoutVisible; }
            set
            {
                _portraitLayoutVisible = value;
                RaisePropertyChanged("PortraitLayoutVisible");
            }
        }

        public Tirinha Tirinha
        {
            get { return _tirinha; }
            set
            {
                _tirinha = value;
                RaisePropertyChanged("Tirinha");
            }
        }
    }
}