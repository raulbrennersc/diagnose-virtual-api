﻿using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class LavouraMap : IEntityTypeConfiguration<Lavoura>
    {
        public void Configure(EntityTypeBuilder<Lavoura> builder)
        {
            //Tabela
            builder.ToTable("lavoura");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Demarcacao)
                .HasColumnName("demarcacao_geom");
            builder.Property(x => x.IdPdi)
                .HasColumnName("id_pdi");
            builder.Property(x => x.Concluida)
                .HasColumnName("concluida")
                .HasDefaultValue(false);
            builder.Property(x => x.Ativa)
                .HasColumnName("ativa")
                .HasDefaultValue(true);
            builder.Property(x => x.Talhoes)
                .HasColumnName("talhoes"); ;

            //Relacoes
            builder.HasOne(x => x.DadosLavoura)
                .WithOne(d => d.Lavoura)
                .HasForeignKey<DadosLavoura>("id_lavoura");
            builder.HasOne(x => x.Etapa)
                .WithMany()
                .HasForeignKey("id_etapa");

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
