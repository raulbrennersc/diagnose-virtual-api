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
                .IsRequired();
            builder.Property(x => x.Proprietario)
                .HasColumnName("proprietario")
                .IsRequired();
            builder.Property(x => x.Gerente)
                .HasColumnName("gerente")
                .IsRequired();
            builder.Property(x => x.Email)
                 .HasColumnName("email")
                 .IsRequired();

            builder.Property(x => x.Telefone)
                 .HasColumnName("telefone")
                 .IsRequired();

            builder.Property(x => x.PontoReferencia)
                .HasColumnName("ponto_referencia")
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
