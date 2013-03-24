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
        private readonly PostsService _postsService;
        private ObservableCollection<Tirinha> _tirinhas;
        private RelayCommand  _maisTirinhas;
        private int _page;
        private int _position;

        private int _selectedIndex;
        private Tirinha _tirinha;
        private bool _loadingData;

        public MainViewModel()
        {
            Tirinhas = new ObservableCollection<Tirinha>();
            _postsService = new PostsService();
            if (IsInDesignMode)
                Tirinha = new Tirinha {Title = "Teste de tirinha grande"};
            else
            {
                SynchronizationContext.Current.Post(state => LoadData(true), null);
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
            get { return _tirinha; }
            set
            {
                _tirinha = value;
                RaisePropertyChanged("Tirinha");
            }
        }

        public ObservableCollection<Tirinha> Tirinhas
        {
            get { return _tirinhas; }
            set
            {
                _tirinhas = value;
                RaisePropertyChanged("Tirinhas");
            }
        }

        public RelayCommand MaisTirinhas
        {
            get { return _maisTirinhas; }
            set
            {
                _maisTirinhas = value;
                RaisePropertyChanged("MaisTirinhas");
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

        public int Position
        {
            get { return _position; }
            set
            {
                _position = value;
                RaisePropertyChanged("Position");
            }
        }

        public async void LoadData(bool primeiraPagina = false)
        {
            try
            {
                if (!_loadingData)
                {
                    _loadingData = true;
                    if (primeiraPagina)
                        _page = 0;
                    var posts = await _postsService.GetPosts(++_page);
                    foreach (var post in posts)
                        Tirinhas.Add(post);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Vida de Programador", MessageBoxButton.OK);
            }
            finally
            {
                _loadingData = false;
            }
        }
    }
}