using System;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using System.Windows;
using AlbertoMonteiroWP7Tools.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VidaDeProgramador.Common.Persistence;
using VidaDeProgramador.Core;

namespace VidaDeProgramador.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly PostsService postsService;
        private bool loadingData;
        private RelayCommand maisTirinhas;
        private int page;
        private int position;

        private ObservableCollection<TirinhaModel> tirinhas;
        private VDPContext vdpContext;

        public MainViewModel(NavigationService navigationService)
        {
            Tirinhas = new ObservableCollection<TirinhaModel>();
            vdpContext = new VDPContext();
            postsService = new PostsService();
            if (!IsInDesignMode)
            {
                SynchronizationContext.Current.Post(state => LoadData(true), null);
                TirinhaSelected = new RelayCommand<TirinhaModel>(tirinha =>
                {
                    IsolatedStorageSettings.ApplicationSettings.Clear();
                    IsolatedStorageSettings.ApplicationSettings.Add("TirinhaCorrent", tirinha);
                    navigationService.NavigateTo("/TirinhaView.xaml");
                    SynchronizationContext.Current.Post(state => tirinha.Nova = false,null);
                });
                MaisTirinhas = new RelayCommand(() => LoadData());
            }
        }

        public RelayCommand<TirinhaModel> TirinhaSelected { get; set; }

        public ObservableCollection<TirinhaModel> Tirinhas
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
                if (!loadingData)
                {
                    loadingData = true;
                    if (primeiraPagina)
                        page = 0;
                    var posts = await postsService.GetPosts(++page,tirinha => !vdpContext.TirinhasLidas.Any(x => x.Link == tirinha.Link));
                    foreach (var post in posts.Select(x => new TirinhaModel(x)))
                        Tirinhas.Add(post);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Vida de Programador", MessageBoxButton.OK);
            }
            finally
            {
                loadingData = false;
            }
        }
    }
}