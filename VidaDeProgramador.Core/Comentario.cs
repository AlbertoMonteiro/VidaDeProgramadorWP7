using System;

namespace VidaDeProgramador.Core
{
    public class Comentario
    {
        public string Autor { get; set; }
        public DateTime Data { get; set; }
        public string Corpo { get; set; }

        public Comentario(string autor, DateTime data, string corpo)
        {
            Autor = autor;
            Data = data;
            Corpo = corpo;
        }
    }
}