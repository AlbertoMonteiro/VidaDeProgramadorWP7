using System.Data.Linq.Mapping;

namespace VidaDeProgramador.Common.Persistence
{
    [Table]
    public class TirinhaLida
    {
        [Column(IsPrimaryKey = true)]
        public string Link { get; set; }
    }
}