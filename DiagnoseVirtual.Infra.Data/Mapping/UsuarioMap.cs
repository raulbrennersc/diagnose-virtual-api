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
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnName("nome");
            builder.Property(x => x.Cpf)
                .IsRequired()
                .HasColumnName("cpf");
            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnName("email");
            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasColumnName("password_hash");
            builder.Property(x => x.PasswordSalt)
                .IsRequired()
                .HasColumnName("password_salt");
            builder.Property(x => x.Ativo)
                .HasDefaultValue(true)
                .HasColumnName("ativo");
            builder.Property(x => x.PrimeiroAcesso)
                .HasDefaultValue(true)
                .HasColumnName("primeiro_acesso");

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
