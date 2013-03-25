using System;
using System.Windows;
using Microsoft.Phone.Controls;
using VidaDeProgramador.Utils;
using VidaDeProgramador.ViewModel;

namespace VidaDeProgramador
{
    public partial class TirinhaView : PhoneApplicationPage
    {
        private const string Formato = "Lendo \"{0}\" do @programadorreal {1} via @VDPWindowsPhone";

        public TirinhaView()
        {
            InitializeComponent();
        }

        private void ShareClick(object sender, EventArgs e)
        {
            try
            {
                var tirinhaViewModel = (TirinhaViewModel)(DataContext);

                CurrentTasks.Current.Share(string.Format(Formato, tirinhaViewModel.Tirinha.Title, tirinhaViewModel.Tirinha.Link));
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