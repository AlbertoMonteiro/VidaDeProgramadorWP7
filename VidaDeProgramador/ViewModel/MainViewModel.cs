using System;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VidaDeProgramador.WordpressApi;

namespace VidaDeProgramador.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly PostsService postsService;
        private ObservableCollection<Post> items;
        private RelayCommand maisTirinhas;
        private int page;
        private int position;

        private int selectedIndex;
        private Post tirinha;

        public MainViewModel()
        {
            postsService = new PostsService();
            Items = new ObservableCollection<Post>();
            if (IsInDesignMode)
            {
                Tirinha = new Post(){Title = "Teste de tirinha grande"};
            } else
            {
                LoadData();
                TirinhaSelected = new RelayCommand<Post>(post =>
                {
                    Tirinha = post;
                    SelectedIndex = 1;
                    Position = 0;
                });
                MaisTirinhas = new RelayCommand(LoadData);
            }
        }

        public RelayCommand<Post> TirinhaSelected { get; set; }

        public Post Tirinha
        {
            get { return tirinha; }
            set
            {
                tirinha = value;
                RaisePropertyChanged("Tirinha");
            }
        }

        public ObservableCollection<Post> Items
        {
            get { return items; }
            set
            {
                items = value;
                RaisePropertyChanged("Items");
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

        public async void LoadData()
        {
            try
            {
                var posts = await postsService.GetPosts(++page);
                foreach (var post in posts)
                    Items.Add(post);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Vida de Programador", MessageBoxButton.OK);
            }
        }
    }
}