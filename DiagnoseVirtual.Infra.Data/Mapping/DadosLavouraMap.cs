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
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(x => x.MesAnoPlantio)
                .HasColumnName("mes_ano_plantio")
                .IsRequired()
                .HasMaxLength(7);
            builder.Property(x => x.Cultivar)
                .HasColumnName("cultivar")
                .IsRequired()
                .HasMaxLength(30);
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
                .IsRequired()
                .HasMaxLength(250);

            //Relacoes
            builder.Property(x => x.IdLavoura)
                .IsRequired()
                .HasColumnName("id_lavoura");

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasIndex(x => x.IdLavoura);

        }
    }
}
