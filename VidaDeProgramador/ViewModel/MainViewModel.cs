using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows;
using AlbertoMonteiroWP7Tools.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using VidaDeProgramador.Persistence;
using VidaDeProgramador.WordpressApi;

namespace VidaDeProgramador.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly PostsService postsService;
        private bool loadingData;
        private RelayCommand maisTirinhas;
        private int page;
        private int position;

        private ObservableCollection<Tirinha> tirinhas;

        public MainViewModel(NavigationService navigationService)
        {
            Tirinhas = new ObservableCollection<Tirinha>();
            postsService = new PostsService(new VDPContext());
            if (!IsInDesignMode)
            {
                SynchronizationContext.Current.Post(state => LoadData(true), null);
                TirinhaSelected = new RelayCommand<Tirinha>(tirinha =>
                {
                    IsolatedStorageSettings.ApplicationSettings.Clear();
                    IsolatedStorageSettings.ApplicationSettings.Add("TirinhaCorrent", tirinha);
                    navigationService.NavigateTo("/TirinhaView.xaml");
                    SynchronizationContext.Current.Post(state => tirinha.Nova = false,null);
                });
                MaisTirinhas = new RelayCommand(() => LoadData());
            }
        }

        public RelayCommand<Tirinha> TirinhaSelected { get; set; }

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
                    IEnumerable<Tirinha> posts = await postsService.GetPosts(++page);
                    foreach (Tirinha post in posts)
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