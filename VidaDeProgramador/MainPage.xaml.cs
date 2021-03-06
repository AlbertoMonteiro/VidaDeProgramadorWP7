﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using VidaDeProgramador.Utils;
using VidaDeProgramador.ViewModel;

namespace VidaDeProgramador
{
    public partial class MainPage
    {
        private double actualHeight;
        private double actualWidth;

        public MainPage()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                var mainViewModel = (MainViewModel) (DataContext);
                mainViewModel.Tirinhas.CollectionChanged += (sender, args) =>
                {
                    var btn = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                    if (btn != null)
                        btn.IsEnabled = !mainViewModel.Tirinhas.Any();
                };
            };
        }

        private void OnPinchDelta(object sender, PinchGestureEventArgs e)
        {
            var image = (Image) sender;
            if (actualWidth == 0)
                actualWidth = image.Width;
            if (actualHeight == 0)
                actualHeight = image.Height;

            var renderTransform = (CompositeTransform) image.RenderTransform;
            if (e.DistanceRatio > 1)
            {
                var distanceRatio = (int) e.DistanceRatio;
                double d = e.DistanceRatio - distanceRatio;
                renderTransform.ScaleX = Math.Min(renderTransform.ScaleX + d, 2);
                renderTransform.ScaleY = Math.Min(renderTransform.ScaleY + d, 2);
            }
            else
            {
                renderTransform.ScaleX = Math.Max(renderTransform.ScaleX - (1 - e.DistanceRatio), 1);
                renderTransform.ScaleY = Math.Max(renderTransform.ScaleY - (1 - e.DistanceRatio), 1);
                if (Math.Abs(renderTransform.ScaleX) == 1)
                {
                    image.Width = actualWidth;
                    image.Height = actualHeight;
                }
            }
            image.Width = image.ActualWidth*renderTransform.ScaleX;
            image.Height = image.ActualHeight*renderTransform.ScaleY;
        }

        private void LoadTirinhas(object sender, EventArgs e)
        {
            var mainViewModel = DataContext as MainViewModel;
            if (mainViewModel != null && !mainViewModel.Tirinhas.Any())
                mainViewModel.LoadData(primeiraPagina: true);
        }

        private void AvaliarClick(object sender, EventArgs e)
        {
            var marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void RecomendacaoClick(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(() => CurrentTasks.Current.Share("Gosta das tirinhas do Vida de Programador? Acompanhe usando a app p/ wp7 do @AIbertoMonteiro http://bit.ly/HrgRp1"));
            }
            catch (InvalidOperationException ex)
            {
                SendExceptionMain(ex);
            }
            catch (Exception ex)
            {
                SendExceptionMain(ex);
            }
        }

        private static void SendExceptionMain(Exception ex)
        {
            var messageBoxResult = MessageBox.Show("Não foi possível abrir o dialog de compartilhamento.\nDeseja enviar o erro ao desenvolvedor da aplicação?", "Vida de Programador", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
                CurrentTasks.Current.SendMail(string.Format("Exception message: {0}\nStacktrace: {1}", ex.InnerException, ex.StackTrace));
        }
    }
}