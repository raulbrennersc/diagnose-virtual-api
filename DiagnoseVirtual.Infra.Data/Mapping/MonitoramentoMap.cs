using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class MonitoramentoMap : IEntityTypeConfiguration<Monitoramento>
    {
        public void Configure(EntityTypeBuilder<Monitoramento> builder)
        {
            //Tabela
            builder.ToTable("monitoramento");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.DataMonitoramento)
                .IsRequired()
                .HasColumnName("data_monitoramento");

            builder.Property(x => x.Ativo)
                .HasColumnName("ativo")
                .HasDefaultValue(true);

            builder.Property(x => x.UrlPdi)
                .HasColumnName("url_pdi")
                .HasDefaultValue(true);

            //Relacoes
            builder.HasMany(x => x.Problemas)
            .WithOne(p => p.Monitoramento)
            .HasForeignKey("id_monitoramento");

            builder.HasMany(x => x.Uploads)
            .WithOne(u => u.Monitoramento)
            .HasForeignKey("id_monitoramento");

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();

            //Regras
            builder.HasQueryFilter(m => m.Ativo);
        }
    }
}