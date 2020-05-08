using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class EtapaLavouraMap : IEntityTypeConfiguration<EtapaLavoura>
    {
        public void Configure(EntityTypeBuilder<EtapaLavoura> builder)
        {
            //Tabela
            builder.ToTable("etapa_lavoura");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired()
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Nome)
                .HasColumnName("nome")
                .IsRequired();

            //Relacoes

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
