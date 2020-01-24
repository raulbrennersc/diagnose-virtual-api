using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Infra.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DiagnoseVirtual.Infra.Data.Context
{
    public class PsqlContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fazenda> Fazendas { get; set; }
        public DbSet<DadosFazenda> DadosFazendas { get; set; }
        public DbSet<DadosLavoura> DadosLavouras { get; set; }
        public DbSet<Lavoura> Lavouras { get; set; }
        public DbSet<LocalizacaoFazenda> LocalizacaoFazendas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseNpgsql("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=qipixel_ark;", x => x.UseNetTopologySuite());

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("diagnose_virtual");
            modelBuilder.Entity<Usuario>(new UsuarioMap().Configure);
            modelBuilder.Entity<Fazenda>(new FazendaMap().Configure);
            modelBuilder.Entity<DadosFazenda>(new DadosFazendaMap().Configure);
            modelBuilder.Entity<LocalizacaoFazenda>(new LocalizacaoFazendaMap().Configure);
            modelBuilder.Entity<Lavoura>(new LavouraMap().Configure);
            modelBuilder.Entity<DadosLavoura>(new DadosLavouraMap().Configure);
            modelBuilder.Entity<Talhao>(new TalhaoMap().Configure);
        }
    }
}
