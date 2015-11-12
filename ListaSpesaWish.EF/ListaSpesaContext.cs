using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListaSpesaWish.EF
{
    public class ListaSpesaContext : DbContext
    {
        public DbSet<ListaSpesa> ListaSpesa { get; set; }
        public DbSet<Utente> Utente { get; set; }
        public DbSet<UtentiListaSpesa> UtentiListaSpesa { get; set; }
        public DbSet<Voce> Voce { get; set; }
        public DbSet<VoceListaSpesa> VoceListaSpesa { get; set; }

        public ListaSpesaContext() : base("ListaSpesaContext")
        {
            Configuration.LazyLoadingEnabled = true;            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Voce>().HasKey(x => x.IdVoce).Property(x => x.IdVoce).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<VoceListaSpesa>().HasKey(x => x.IdVoceListaSpesa).Property(x => x.IdVoceListaSpesa).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Utente>().HasKey(x => x.IdUtente).Property(x => x.IdUtente).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<UtentiListaSpesa>().HasKey(x => x.IdUtentiListaSpesa).Property(x => x.IdUtentiListaSpesa).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ListaSpesa>().HasKey(x => x.IdListaSpesa).Property(x => x.IdListaSpesa).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ListaSpesa>().HasMany(x => x.UtentiListaSpesa).WithRequired(x => x.ListaSpesa);
            modelBuilder.Entity<ListaSpesa>().HasMany(x => x.VociListaSpesa).WithRequired(x => x.ListaSpesa);
            modelBuilder.Entity<Voce>().HasOptional(x => x.VoceListaSpesa).WithMany(x => x.Voci);
            modelBuilder.Entity<Utente>().HasMany(x => x.UtentiListaSpesa).WithRequired(x => x.Utente);

            base.OnModelCreating(modelBuilder);
        }
    }
}
