using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class TalhaoMap : IEntityTypeConfiguration<Talhao>
    {
        public void Configure(EntityTypeBuilder<Talhao> builder)
        {
            //Tabela
            builder.ToTable("talhao");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Geometria)
                .IsRequired()
                .HasColumnName("geometria_geom");

            //Relacoes

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasIndex(x => x.IdLavoura);

        }
    }
}
