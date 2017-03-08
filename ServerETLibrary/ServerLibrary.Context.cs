
namespace ServerETLibrary
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LibraryDatabaseEntities : DbContext
    {
        public LibraryDatabaseEntities()
            : base("name=LibraryDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Angajati> Angajatis { get; set; }
        public virtual DbSet<Carti> Cartis { get; set; }
        public virtual DbSet<Clienti> Clientis { get; set; }
        public virtual DbSet<Imprumuturi> Imprumuturis { get; set; }
    }
}
