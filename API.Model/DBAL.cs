using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Model
{
    public class DBAL
    {
        public static readonly Entities db = new Entities();
        public DBAL() {
            db.Database.CommandTimeout = 1000000;
            db.Configuration.ProxyCreationEnabled = false; }
    }
}
