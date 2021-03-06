using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class ProblemaMonitoramentoMap : IEntityTypeConfiguration<ProblemaMonitoramento>
    {
        public void Configure(EntityTypeBuilder<ProblemaMonitoramento> builder)
        {
            //Tabela
            builder.ToTable("problema_monitoramento");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Descricao)
                .HasColumnName("descricao");

            builder.Property(x => x.Recomendacao)
                .HasColumnName("recomendacao");

            builder.Property(x => x.Ponto)
                .HasColumnName("ponto_geom");

            builder.Property(x => x.Imagens)
                .HasColumnName("imagens");

            //Relacoes

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}