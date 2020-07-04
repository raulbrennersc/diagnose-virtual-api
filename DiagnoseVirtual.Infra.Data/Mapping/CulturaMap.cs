using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class CulturaMap : IEntityTypeConfiguration<Cultura>
    {
        public void Configure(EntityTypeBuilder<Cultura> builder)
        {
            //Tabela
            builder.ToTable("cultura");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Nome)
                .HasColumnName("nome");

            //Relacoes

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
