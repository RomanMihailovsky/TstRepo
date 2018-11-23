using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;

using InvCtrl.Entities;

namespace InvCtrl.Concrete
{
    class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("Ikur20171023_")            
        {
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Sale> Sales { get; set; }

    }
}
