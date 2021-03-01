using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace Model
{
    public class CsmContext : DbContext
    {
        public CsmContext(string connection) : base(connection)
        {
            Database.SetInitializer<CsmContext>(new DropCreateDatabaseIfModelChanges<CsmContext>());
            Database.Initialize(force: true);

            //Database.SetInitializer<SchoolDBContext>(new CreateDatabaseIfNotExists<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseIfModelChanges<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseAlways<SchoolDBContext>());
            //Database.SetInitializer<SchoolDBContext>(new SchoolDBInitializer());
        }

        public DbSet<Authentication> authentications { get; set; }
    }
}
