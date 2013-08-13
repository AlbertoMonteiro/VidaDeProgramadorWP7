using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls;
using VidaDeProgramador.Common.Persistence;
using VidaDeProgramador.Core;
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
        private PostsService postsService;

        public TirinhaViewModel(NavigationService navigationService)
        {
            this.navigationService = navigationService;
            Comentarios=new ObservableCollection<Comentario>();
            if (IsInDesignMode)
            {
                Tirinha = new TirinhaModel();
                Comentarios.Add(new Comentario("TioDavid", DateTime.Parse("Mon, 12 Aug 2013 13:12:55 +0000"), @"Se bem que…
Complexo é o que não é simples. E fácil é o que não é difícil.
Há uma diferença. Pois tem coisa que é simples, mas não é fácil e coisa fáceis mas complexas, ou seja, que apenas tomam mais tempo e etapas.

já imagino alguns thumb down, mas a realidade é essa xD
Ainda assim, ri muito com a tirinha"));
            }
            else
            {
                postsService = new PostsService();
                vdpContext = new VDPContext();
                this.navigationService.Navigated += AtualizaTirinha;
                PropertyChanged += async (sender, args) =>
                {
                    if (args.PropertyName == "SelectedIndex" && SelectedIndex == 2)
                    {
                        Comentarios.Clear();
                        var comentarios = await postsService.GetComments(Tirinha.LinkComentarios);
                        foreach (var comment in comentarios)
                        {
                            Comentarios.Add(comment);
                        }
                    }
                };
                OrientacaoAlterada = new RelayCommand<PageOrientation>(pageOrientation =>
                {
                    LandscapeLayoutVisible = pageOrientation == PageOrientation.LandscapeLeft || pageOrientation == PageOrientation.LandscapeRight ? Visibility.Visible :  Visibility.Collapsed;
                    PortraitLayoutVisible = pageOrientation == PageOrientation.PortraitUp ? Visibility.Visible :  Visibility.Collapsed;
                });
            }
        }

        public ObservableCollection<Comentario> Comentarios { get; set; }

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