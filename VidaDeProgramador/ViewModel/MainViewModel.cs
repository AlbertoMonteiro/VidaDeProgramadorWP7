using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VidaDeProgramador.WordpressApi;

namespace VidaDeProgramador.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly PostsService postsService;
        private ObservableCollection<Tirinha> tirinhas;
        private RelayCommand maisTirinhas;
        private int page;
        private int position;

        private int selectedIndex;
        private Tirinha tirinha;

        public MainViewModel()
        {
            Tirinhas = new ObservableCollection<Tirinha>();
            postsService = new PostsService();
            if (IsInDesignMode)
                Tirinha = new Tirinha {Title = "Teste de tirinha grande"};
            else
            {
                SynchronizationContext.Current.Post(state => LoadData(), null);
                TirinhaSelected = new RelayCommand<Tirinha>(post =>
                {
                    Tirinha = post;
                    SelectedIndex = 1;
                    Position = 0;
                });
                MaisTirinhas = new RelayCommand(() => LoadData());
            }
        }

        public RelayCommand<Tirinha> TirinhaSelected { get; set; }

        public Tirinha Tirinha
        {
            get { return tirinha; }
            set
            {
                tirinha = value;
                RaisePropertyChanged("Tirinha");
            }
        }

        public ObservableCollection<Tirinha> Tirinhas
        {
            get { return tirinhas; }
            set
            {
                tirinhas = value;
                RaisePropertyChanged("Tirinhas");
            }
        }

        public RelayCommand MaisTirinhas
        {
            get { return maisTirinhas; }
            set
            {
                maisTirinhas = value;
                RaisePropertyChanged("MaisTirinhas");
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

        public int Position
        {
            get { return position; }
            set
            {
                position = value;
                RaisePropertyChanged("Position");
            }
        }

        public async void LoadData(bool primeiraPagina = false)
        {
            try
            {
                if (primeiraPagina)
                    page = 0;
                var posts = await postsService.GetPosts(++page);
                foreach (var post in posts)
                    Tirinhas.Add(post);
            } catch (Exception e)
            {
                MessageBox.Show(e.Message, "Vida de Programador", MessageBoxButton.OK);
            }
        }
    }
}