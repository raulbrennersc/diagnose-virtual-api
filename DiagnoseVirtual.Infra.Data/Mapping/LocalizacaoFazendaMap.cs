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
            builder.Property(x => x.Proprietario)
                .HasColumnName("proprietario")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Gerente)
                .HasColumnName("gerente")
                .HasMaxLength(30)
                .IsRequired();
            builder.Property(x => x.Email)
                 .HasColumnName("email")
                 .HasMaxLength(50)
                 .IsRequired();

            builder.Property(x => x.Telefone)
                 .HasColumnName("telefone")
                 .HasMaxLength(20)
                 .IsRequired();

            builder.Property(x => x.PontoReferencia)
                .HasColumnName("ponto_referencia")
                .HasMaxLength(70)
                .IsRequired();

            //Relacoes
            builder.HasOne(x => x.Municipio)
                .WithMany()
                .HasForeignKey("id_municipio");

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
