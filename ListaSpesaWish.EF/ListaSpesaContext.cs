using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;

namespace ListaSpesaWish.EF
{
    public class ListaSpesaContext : DbContext
    {
        private NLog.Logger _log = NLog.LogManager.GetLogger("apiLog");

        public DbSet<ListaSpesa> ListaSpesa { get; set; }
        public DbSet<Utente> Utente { get; set; }
        public DbSet<UtentiListaSpesa> UtentiListaSpesa { get; set; }
        public DbSet<Voce> Voce { get; set; }
        public DbSet<VoceListaSpesa> VoceListaSpesa { get; set; }

        public ListaSpesaContext() : base("name=ListaSpesaContext")
        {   
            //Database.SetInitializer<ListaSpesaContext>(new DbInitializer());
            
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = true;

            Database.Log = s => _log.Info(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Voce>()
                        .ToTable("Voce")
                        .HasKey(x => x.IdVoce)
                        .Property(x => x.IdVoce)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<VoceListaSpesa>()
                        .ToTable("VoceListaSpesa")
                        .HasKey(x => x.IdVoceListaSpesa)
                        .Property(x => x.IdVoceListaSpesa)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Utente>()
                        .ToTable("Utente").HasKey(x => x.IdUtente)
                        .Property(x => x.IdUtente)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<UtentiListaSpesa>()
                        .ToTable("UtentiListaSpesa")
                        .HasKey(x => x.IdUtentiListaSpesa)
                        .Property(x => x.IdUtentiListaSpesa)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ListaSpesa>()
                        .ToTable("ListaSpesa")
                        .HasKey(x => x.IdListaSpesa)
                        .Property(x => x.IdListaSpesa)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ListaSpesa>()
                        .HasMany(x => x.UtentiListaSpesa)
                        .WithRequired(x => x.ListaSpesa);

            modelBuilder.Entity<ListaSpesa>()
                        .HasMany(x => x.VociListaSpesa)
                        .WithRequired(x => x.ListaSpesa);

            modelBuilder.Entity<Voce>()
                        .HasMany(x => x.VociListaSpesa)
                        .WithRequired(x => x.Voce);

            modelBuilder.Entity<Utente>()
                        .HasMany(x => x.UtentiListaSpesa)
                        .WithRequired(x => x.Utente);

            base.OnModelCreating(modelBuilder);
        }
    }
}
