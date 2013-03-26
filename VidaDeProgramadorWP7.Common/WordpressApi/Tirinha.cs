using System;

namespace VidaDeProgramador.Common.WordpressApi
{
    public class Tirinha
    {
        public Tirinha()
        {
            Nova = true;
        }

        public Tirinha(string title, string body, string image, string link, DateTime publicadoEm, string linkComentarios, int totalComentarios)
        {
            Title = title;
            Body = body;
            Image = image;
            Link = link;
            PublicadoEm = publicadoEm;
            LinkComentarios = linkComentarios;
            TotalComentarios = totalComentarios;
        }

        public string Title { get; set; }

        public string Image { get; set; }

        public string Body { get; set; }

        public string Link { get; set; }

        public DateTime PublicadoEm { get; set; }

        public string LinkComentarios { get; set; }

        public int TotalComentarios { get; set; }

        public bool Nova { get; set; }
    }
}