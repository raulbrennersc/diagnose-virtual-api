using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class LocalizacaoFazendaMap : IEntityTypeConfiguration<LocalizacaoFazenda>
    {
        public void Configure(EntityTypeBuilder<LocalizacaoFazenda> builder)
        {
            //Tabela
            builder.ToTable("localizacao_fazenda");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired()
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Nome)
                .HasColumnName("nome")
                .HasMaxLength(30)
                .IsRequired();
            builder.Property(x => x.Estado)
                .HasColumnName("estado")
                .HasMaxLength(20)
                .IsRequired();
            builder.Property(x => x.Municipio)
                .HasColumnName("municipio")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Proprietario)
                .HasColumnName("proprietario")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Gerente)
                .HasColumnName("gerente")
                .HasMaxLength(30)
                .IsRequired();
            builder.Property(x => x.Contato)
                .HasColumnName("contato")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.PontoReferencia)
                .HasColumnName("ponto_referencia")
                .HasMaxLength(70)
                .IsRequired();

            //Relacoes

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();

        }
    }
}
