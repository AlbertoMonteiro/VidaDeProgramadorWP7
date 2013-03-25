using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VidaDeProgramador.WordpressApi;

namespace VidaDeProgramador.Converters
{
    public class TirinhaToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tirinha = (Tirinha) value;

            var bitmapImage = new BitmapImage(new Uri(tirinha.Image))
            {
                CreateOptions = BitmapCreateOptions.None
            };
            var novoImage = new BitmapImage(new Uri(@"/novo.png", UriKind.Relative))
            {
                CreateOptions = BitmapCreateOptions.None
            };

            bitmapImage.ImageOpened += (o, args) =>
            {
                const int tileTamanho = 200;
                const int novoTamanho = 34;

                var tirinhaImage = new WriteableBitmap(bitmapImage);
                var tile = new WriteableBitmap(tileTamanho, tileTamanho);
                var titulo = new WriteableBitmap(tileTamanho, tileTamanho);

                tirinhaImage = tirinhaImage.Resize(tileTamanho, tileTamanho, WriteableBitmapExtensions.Interpolation.Bilinear);

                var rect = new Rect(0.0, 0.0, tileTamanho, tileTamanho);

                tile.Blit(rect, tirinhaImage, rect, WriteableBitmapExtensions.BlendMode.None);

                if (tirinha.Nova)
                {
                    var novo = new WriteableBitmap(novoImage);

                    int x = tileTamanho - novoTamanho - 1;

                    var novoDestiny = new Rect(0, 0, novoTamanho, novoTamanho);
                    var novoPosition = new Rect(x, 1, novoTamanho, novoTamanho);

                    tile.Blit(novoPosition, novo, novoDestiny, WriteableBitmapExtensions.BlendMode.Alpha); 
                }

                var txt1 = new TextBlock
                {
                    Text = tirinha.Title,
                    Width = tileTamanho*0.80,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    FontSize = 18,
                    FontFamily = new FontFamily("Sergoe UI"),
                    Foreground = new SolidColorBrush(Colors.White)
                };

                const int xDe = (int) (tileTamanho*0.10);
                int yDe = tileTamanho - ((int) txt1.ActualHeight) - 10;

                const int xAte = (int) (tileTamanho*0.90);
                const int yAte = tileTamanho - 10;

                titulo.FillRectangle(xDe, yDe, xAte, yAte, Color.FromArgb(204, 0, 71, 171));
                titulo.DrawRectangle(xDe - 1, yDe - 1, xAte + 1, yAte + 1, Colors.Black);

                tile.Blit(rect, titulo, rect, WriteableBitmapExtensions.BlendMode.Alpha);

                var bitmap = new WriteableBitmap(txt1, null);

                tile.Blit(new Rect(xDe, yDe, xAte - xDe, yAte - yDe), bitmap, new Rect(0, 0, xAte - xDe, yAte - yDe), WriteableBitmapExtensions.BlendMode.Alpha);

                var ms = new MemoryStream();
                tile.SaveJpeg(ms, tileTamanho, tileTamanho, 0, 100);
                ms.Seek(0, SeekOrigin.Begin);
                bitmapImage.SetSource(ms);
            };

            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}