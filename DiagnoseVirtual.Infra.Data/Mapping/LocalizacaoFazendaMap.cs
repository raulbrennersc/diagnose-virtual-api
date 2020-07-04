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
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Nome)
                .HasColumnName("nome");
            builder.Property(x => x.Proprietario)
                .HasColumnName("proprietario");
            builder.Property(x => x.Gerente)
                .HasColumnName("gerente");
            builder.Property(x => x.Email)
                 .HasColumnName("email");

            builder.Property(x => x.Telefone)
                 .HasColumnName("telefone");

            builder.Property(x => x.PontoReferencia)
                .HasColumnName("ponto_referencia");

            //Relacoes
            builder.HasOne(x => x.Municipio)
                .WithMany()
                .HasForeignKey("id_municipio");

            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
