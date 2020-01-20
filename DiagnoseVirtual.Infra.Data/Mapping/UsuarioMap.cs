using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id");

            builder.Property(u => u.Cpf)
                .IsRequired()
                .HasColumnName("cpf");

            builder.Property(c => c.Email)
                .IsRequired()
                .HasColumnName("email");

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnName("nome");

            builder.Property(c => c.PasswordHash)
                .IsRequired()
                .HasColumnName("password_hash");

            builder.Property(c => c.PasswordSalt)
                .IsRequired()
                .HasColumnName("password_salt");
        }
    }
}
