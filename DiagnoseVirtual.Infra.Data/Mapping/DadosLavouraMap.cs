using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class DadosLavouraMap : IEntityTypeConfiguration<DadosLavoura>
    {
        public void Configure(EntityTypeBuilder<DadosLavoura> builder)
        {
            //Tabela
            builder.ToTable("dados_lavoura");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired()
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Nome)
                .HasColumnName("nome")
                .IsRequired();
            builder.Property(x => x.MesAnoPlantio)
                .HasColumnName("mes_ano_plantio")
                .IsRequired();
            builder.Property(x => x.Cultivar)
                .HasColumnName("cultivar")
                .IsRequired();
            builder.Property(x => x.AreaTotal)
                .HasColumnName("area_total")
                .IsRequired();
            builder.Property(x => x.NumeroPlantas)
                .HasColumnName("numero_plantas")
                .IsRequired();
            builder.Property(x => x.EspacamentoVertical)
                .HasColumnName("expacamento_vertical")
                .IsRequired();
            builder.Property(x => x.EspacamentoHorizontal)
                .HasColumnName("espacamento_horizontal")
                .IsRequired();
            builder.Property(x => x.Observacoes)
                .HasColumnName("observacoes")
                .IsRequired();

            //Relacoes

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
