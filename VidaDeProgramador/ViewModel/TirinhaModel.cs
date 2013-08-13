using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VidaDeProgramador.Core;

namespace VidaDeProgramador.ViewModel
{
    public class TirinhaModel : INotifyPropertyChanged
    {
        private string body;
        private string image;
        private string link;
        private string linkComentarios;
        private bool nova;
        private DateTime publicadoEm;
        private string title;
        private int totalComentarios;
        private readonly BitmapImage novoImage;
        private WriteableBitmap novo;
        const int TileTamanho = 200;
        const int NovoTamanho = 34;

        public TirinhaModel()
        {
            Nova = true;

            novoImage = new BitmapImage(new Uri(@"/novo.png", UriKind.Relative))
            {
                CreateOptions = BitmapCreateOptions.None
            };
            novoImage.ImageOpened += (sender, args) => novo = new WriteableBitmap(novoImage);
        }

        public TirinhaModel(Tirinha tirinha)
            :this()
        {
            title = tirinha.Title;
            body = tirinha.Body;
            image = tirinha.Image;
            link = tirinha.Link;
            publicadoEm = tirinha.PublicadoEm;
            linkComentarios = tirinha.LinkComentarios;
            totalComentarios = tirinha.TotalComentarios;
            nova = tirinha.Nova;
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
                OnPropertyChanged("BitmapImagem");
            }
        }

        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
                OnPropertyChanged("BitmapImagem");
            }
        }

        public string Body
        {
            get { return body; }
            set
            {
                body = value;
                OnPropertyChanged("Body");
            }
        }

        public string Link
        {
            get { return link; }
            set
            {
                link = value;
                OnPropertyChanged("Link");
            }
        }

        public DateTime PublicadoEm
        {
            get { return publicadoEm; }
            set
            {
                publicadoEm = value;
                OnPropertyChanged("PublicadoEm");
            }
        }

        public string LinkComentarios
        {
            get { return linkComentarios; }
            set
            {
                linkComentarios = value;
                OnPropertyChanged("LinkComentarios");
            }
        }

        public int TotalComentarios
        {
            get { return totalComentarios; }
            set
            {
                totalComentarios = value;
                OnPropertyChanged("TotalComentarios");
            }
        }

        public bool Nova
        {
            get { return nova; }
            set
            {
                nova = value;
                OnPropertyChanged("Nova");
                OnPropertyChanged("BitmapImagem");
            }
        }

        public BitmapImage BitmapImagem
        {
            get
            {
                var bitmapImage = new BitmapImage(new Uri(Image))
                {
                    CreateOptions = BitmapCreateOptions.None
                };

                bitmapImage.ImageOpened += (o, args) =>
                {
                    var tirinhaImage = new WriteableBitmap(bitmapImage);
                    var tile = new WriteableBitmap(TileTamanho, TileTamanho);
                    var titulo = new WriteableBitmap(TileTamanho, TileTamanho);

                    tirinhaImage = tirinhaImage.Resize(TileTamanho, TileTamanho, WriteableBitmapExtensions.Interpolation.Bilinear);

                    var rect = new Rect(0.0, 0.0, TileTamanho, TileTamanho);

                    tile.Blit(rect, tirinhaImage, rect, WriteableBitmapExtensions.BlendMode.None);

                    if (Nova)
                    {
                        int x = TileTamanho - NovoTamanho - 1;

                        var novoDestiny = new Rect(0, 0, NovoTamanho, NovoTamanho);
                        var novoPosition = new Rect(x, 1, NovoTamanho, NovoTamanho);

                        tile.Blit(novoPosition, novo, novoDestiny, WriteableBitmapExtensions.BlendMode.Alpha);
                    }

                    var txt1 = new TextBlock
                    {
                        Text = Title,
                        Width = TileTamanho * 0.80,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 18,
                        FontFamily = new FontFamily("Sergoe UI"),
                        Foreground = new SolidColorBrush(Colors.White)
                    };

                    const int xDe = (int)(TileTamanho * 0.10);
                    int yDe = TileTamanho - ((int)txt1.ActualHeight) - 10;

                    const int xAte = (int)(TileTamanho * 0.90);
                    const int yAte = TileTamanho - 10;

                    titulo.FillRectangle(xDe, yDe, xAte, yAte, Color.FromArgb(204, 0, 71, 171));
                    titulo.DrawRectangle(xDe - 1, yDe - 1, xAte + 1, yAte + 1, Colors.Black);

                    tile.Blit(rect, titulo, rect, WriteableBitmapExtensions.BlendMode.Alpha);

                    var bitmap = new WriteableBitmap(txt1, null);

                    tile.Blit(new Rect(xDe, yDe, xAte - xDe, yAte - yDe), bitmap, new Rect(0, 0, xAte - xDe, yAte - yDe), WriteableBitmapExtensions.BlendMode.Alpha);

                    using (var ms = new MemoryStream())
                    {
                        tile.SaveJpeg(ms, TileTamanho, TileTamanho, 0, 100);
                        ms.Seek(0, SeekOrigin.Begin);
                        bitmapImage.SetSource(ms);
                    }
                };

                return bitmapImage;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
