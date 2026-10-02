using System.Data.Entity;

namespace Personenverwaltung.Data
{
    public class PersonenDbContext : DbContext
    {
        public PersonenDbContext() : base("name=Default")
        {
            Database.SetInitializer<PersonenDbContext>(null);
        }

        public DbSet<Person> Personen { get; set; }
        public DbSet<Anschrift> Anschriften { get; set; }
        public DbSet<Telefonverbindung> Telefonverbindungen { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Person", "dbo");
            modelBuilder.Entity<Anschrift>().ToTable("Anschrift", "dbo");
            modelBuilder.Entity<Telefonverbindung>().ToTable("Telefonverbindung", "dbo");

            modelBuilder.Entity<Person>().Property(p => p.Geburtsdatum).HasColumnType("date");
            modelBuilder.Entity<Anschrift>().Property(a => a.Straße).HasColumnName("Straße");
            
            modelBuilder.Entity<Anschrift>()
                .HasRequired(a => a.Person)
                .WithMany(p => p.Anschriften)
                .HasForeignKey(a => a.PersonId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Telefonverbindung>()
                .HasRequired(t => t.Person)
                .WithMany(p => p.Telefonverbindungen)
                .HasForeignKey(t => t.PersonId)
                .WillCascadeOnDelete(false);
        }
    }
}