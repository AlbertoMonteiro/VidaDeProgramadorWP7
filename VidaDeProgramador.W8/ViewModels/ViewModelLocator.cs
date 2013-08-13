using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VidaDeProgramador.Core;

namespace VidaDeProgramador.W8.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            Main = new MainViewModel();
        }

        public static MainViewModel Main { get; set; }
    }

    public class MainViewModel
    {
        private SynchronizationContext synchronizationContext;

        public MainViewModel()
        {
            Tirinhas = new ObservableCollection<Tirinha>();

            CarregaTirinhas();
        }

        public async void CarregaTirinhas()
        {
            synchronizationContext = SynchronizationContext.Current;
            var p = new PostsService();
            var tirinhas = await p.GetPosts(0, tirinha => true);
            foreach (var tirinha in tirinhas)
                synchronizationContext.Post(state => { Tirinhas.Add(tirinha); }, null);
        }

        public ObservableCollection<Tirinha> Tirinhas { get; set; }
    }
}
