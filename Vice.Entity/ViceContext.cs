using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Vice.Entity
{
    public partial class ViceDataContext : DbContext
    {
        public ViceDataContext()
            : base("Admin_Sqlserver")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ViceDataContext>());
            Database.SetInitializer(new CreateDatabaseIfNotExists<ViceDataContext>());
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Database.SetInitializer<ViceDataContext>(new AdminInitializer());

            this.Database.Initialize(false);
        }

        public DbSet<Base_Category> Base_Category { get; set; }

        public DbSet<Base_Article> Base_Article { get; set; }

        public DbSet<Base_Schedule> Base_Duty { get; set; }

        public DbSet<Base_ImageLinks> Base_ImageLinks { get; set; }

        public DbSet<Base_ScrollImage> Base_ScrollImage { get; set; }

        public DbSet<Base_SuperStar> BaseSuperStar { get; set; }

        public DbSet<Base_User> Base_User { get; set; }

        public DbSet<Base_Notify> Base_Notify { get; set; }

    }
}
