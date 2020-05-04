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
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Cultura> Culturas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseNpgsql("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=diagnosys;", x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("diagnose_virtual");
            var assembly = typeof(Usuario).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            modelBuilder.Entity<Usuario>(new UsuarioMap().Configure);
            modelBuilder.Entity<Fazenda>(new FazendaMap().Configure);
            modelBuilder.Entity<DadosFazenda>(new DadosFazendaMap().Configure);
            modelBuilder.Entity<LocalizacaoFazenda>(new LocalizacaoFazendaMap().Configure);
            modelBuilder.Entity<Lavoura>(new LavouraMap().Configure);
            modelBuilder.Entity<DadosLavoura>(new DadosLavouraMap().Configure);
            modelBuilder.Entity<Talhao>(new TalhaoMap().Configure);
            modelBuilder.Entity<Monitoramento>(new MonitoramentoMap().Configure);
            modelBuilder.Entity<ProblemaMonitoramento>(new ProblemaMonitoramentoMap().Configure);
            modelBuilder.Entity<UploadMonitoramento>(new UploadMonitoramentoMap().Configure);
            modelBuilder.Entity<Estado>(new EstadoMap().Configure);
            modelBuilder.Entity<Municipio>(new MunicipioMap().Configure);
            modelBuilder.Entity<Cultura>(new CulturaMap().Configure);
        }
    }
}