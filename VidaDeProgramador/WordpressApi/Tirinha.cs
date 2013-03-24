using System;

namespace VidaDeProgramador.WordpressApi
{
    public class Tirinha
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public DateTime PublicadoEm { get; set; }
        public string LinkComentarios { get; set; }
        public int TotalComentarios { get; set; }
    }
}