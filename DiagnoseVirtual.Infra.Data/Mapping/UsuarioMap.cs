using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class UsuarioMap : ClassMapping<Usuario>, IEntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            Table("usuario");
            Schema("diagnose_virtual");

            ManyToOne(x => x.Fazendas, m =>
            {
                m.Column("id_usuario");


                m.Cascade(Cascade.All | Cascade.None | Cascade.Persist | Cascade.Remove);
                m.Fetch(FetchKind.Join);
                m.Update(true);
                m.Insert(true);
                //m.Access(Accessor.Field);
                m.Unique(true);
                m.OptimisticLock(true);

                m.Lazy(LazyRelation.Proxy);
                m.NotNullable(true);
            });

            Set(x => x.Fazendas, c =>
            {
                ManyToOne(x => x.Fazendas, m =>
                {
                    m.Column("id_usuario");


                    m.Cascade(Cascade.All | Cascade.None | Cascade.Persist | Cascade.Remove);
                    m.Fetch(FetchKind.Join);
                    m.Update(true);
                    m.Insert(true);
                    //m.Access(Accessor.Field);
                    m.Unique(true);
                    m.OptimisticLock(true);

                    m.Lazy(LazyRelation.Proxy);
                    m.NotNullable(true);
                });

            }, r =>
            {
                r.OneToMany(m =>
                {
                    m.NotFound(NotFoundMode.Exception); // or NotFoundMode.Ignore
                    m.Class(typeof(Usuario));
                    m.EntityName("Usuario");
                    m.Class(typeof(Usuario));
                });
            });

            Id(x => x.Id, id =>
            {
                id.Generator(NHibernate.Mapping.ByCode.Generators.Increment);
            });

            Property(x => x.Nome, p =>
            {
                p.Column("nome");
            });

            Property(x => x.Cpf, p =>
            {
                p.Column("cpf");
            });

            Property(x => x.Email, p =>
            {
                p.Column("email");
            });

            Property(x => x.PasswordHash, p =>
            {
                p.Column("password_hash");
            });

            Property(x => x.PasswordSalt, p =>
            {
                p.Column("password_salt");
            });

            Property(x => x.Ativo, p =>
            {
                p.Column("ativo");
            });
        }

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
                .HasColumnName("nome")
                .HasMaxLength(100);
            builder.Property(x => x.Cpf)
                .IsRequired()
                .HasColumnName("cpf")
                .HasMaxLength(11);
            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(50);
            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasColumnName("password_hash");
            builder.Property(x => x.PasswordSalt)
                .IsRequired()
                .HasColumnName("password_salt");
            builder.Property(x => x.Ativo)
                .HasDefaultValue(true)
                .HasColumnName("ativo");

            //Relacoes
            builder.HasMany(x => x.Fazendas)
                .WithOne(f => f.Usuario)
                .HasForeignKey(f => f.IdUsuario);

            //Indices e uniques
            builder.HasIndex(x => x.Cpf).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
