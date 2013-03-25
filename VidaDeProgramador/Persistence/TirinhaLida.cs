using System.Data.Linq.Mapping;

namespace VidaDeProgramador.Persistence
{
    [Table]
    public class TirinhaLida
    {
        [Column(IsPrimaryKey = true)]
        public string Link { get; set; }
    }
}