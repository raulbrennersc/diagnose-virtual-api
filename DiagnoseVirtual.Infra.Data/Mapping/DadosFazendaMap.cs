using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class DadosFazendaMap : IEntityTypeConfiguration<DadosFazenda>
    {
        public void Configure(EntityTypeBuilder<DadosFazenda> builder)
        {
            //Tabela
            builder.ToTable("dados_fazenda");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.Property(x => x.AreaTotal)
                .HasColumnName("area_total");
            builder.Property(x => x.QuantidadeLavouras)
                .HasColumnName("quantidade_lavouras");

            //Relacoes
            builder.HasOne(x => x.Cultura)
                .WithMany()
                .HasForeignKey("id_cultura");

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
