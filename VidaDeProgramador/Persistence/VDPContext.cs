using System.Data.Linq;

namespace VidaDeProgramador.Persistence
{
    public class VDPContext : DataContext
    {
        public VDPContext()
            : base("Data source=isostore:/Db.sdf")
        {
            if (!DatabaseExists())
                CreateDatabase();
        }

        public Table<TirinhaLida> TirinhasLidas { get { return GetTable<TirinhaLida>(); } }
    }
}
