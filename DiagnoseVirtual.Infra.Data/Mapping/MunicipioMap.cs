using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class MunicipioMap : IEntityTypeConfiguration<Municipio>
    {
        public void Configure(EntityTypeBuilder<Municipio> builder)
        {
            //Tabela
            builder.ToTable("municipio");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Nome)
                .HasColumnName("nome");
            builder.Property(x => x.CodigoIbge)
                .HasColumnName("codigo_ibge");

            //Relacoes
            builder.HasOne(x => x.Estado)
                .WithMany(e => e.Municipios)
                .HasForeignKey("id_estado");

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
