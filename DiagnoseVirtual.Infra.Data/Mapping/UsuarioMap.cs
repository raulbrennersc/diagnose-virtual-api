using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            //Tabela
            builder.ToTable("usuario");
            builder.HasKey(x => x.Id);

            //Propriedades
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Nome)
                .HasColumnName("nome");
            builder.Property(x => x.Cpf)
                .HasColumnName("cpf");
            builder.Property(x => x.Email)
                .HasColumnName("email");
            builder.Property(x => x.PasswordHash)
                .HasColumnName("password_hash");
            builder.Property(x => x.PasswordSalt)
                .HasColumnName("password_salt");
            builder.Property(x => x.Ativo)
                .HasDefaultValue(true)
                .HasColumnName("ativo");
            builder.Property(x => x.PrimeiroAcesso)
                .HasDefaultValue(true)
                .HasColumnName("primeiro_acesso");
            builder.Property(x => x.DataCadastro)
                .HasColumnName("data_cadastro");

            //Relacoes
            builder.HasMany(x => x.Fazendas)
                .WithOne(f => f.Usuario)
                .HasForeignKey("id_usuario");

            //Indices e uniques
            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasIndex(x => x.Cpf).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
