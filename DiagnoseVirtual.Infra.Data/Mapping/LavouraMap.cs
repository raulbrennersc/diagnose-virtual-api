using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

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
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Demarcacao)
                .HasColumnName("demarcacao_geom")
                .IsRequired();
            builder.Property(x => x.Concluida)
                .HasColumnName("concluida")
                .HasDefaultValue(false);

            //Relacoes
            builder.HasOne(x => x.DadosLavoura)
                .WithOne(d => d.Lavoura)
                .HasForeignKey<DadosLavoura>(d => d.IdLavoura);
            builder.HasMany(x => x.Talhoes)
                .WithOne(t => t.Lavoura)
                .HasForeignKey(t => t.IdLavoura);

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasIndex(x => x.IdFazenda);
        }
    }
}
