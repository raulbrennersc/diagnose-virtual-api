using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class EtapaFazendaMap : IEntityTypeConfiguration<EtapaFazenda>
    {
        public void Configure(EntityTypeBuilder<EtapaFazenda> builder)
        {
            //Tabela
            builder.ToTable("etapa_fazenda");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Nome)
                .HasColumnName("nome");

            //Relacoes

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
