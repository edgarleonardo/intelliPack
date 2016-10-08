using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliPack.DataAccessLayer.Context
{
    public class SourcePaqContextDb : DbContext
    {
        public SourcePaqContextDb() : base("SourcePaqContextDb")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}
