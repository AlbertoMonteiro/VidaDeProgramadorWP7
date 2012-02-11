using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VidaDeProgramador.WordpressApi;

namespace VidaDeProgramador.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Post> items;
        private int position;
        private int page;

        private int selectedIndex;
        private Post tirinha;
        private readonly PostsService postsService;

        public MainViewModel()
        {
            postsService = new PostsService();
            Items = new ObservableCollection<Post>();
            if (IsInDesignMode)
            {
                
            } else
            {
                LoadData();
                TirinhaSelected = new RelayCommand<Post>(post =>
                {
                    Tirinha = post;
                    SelectedIndex = 1;
                    Position = 0;
                });
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
            Items = new ObservableCollection<Post>(await postsService.GetPosts(++page));
        }
    }
}