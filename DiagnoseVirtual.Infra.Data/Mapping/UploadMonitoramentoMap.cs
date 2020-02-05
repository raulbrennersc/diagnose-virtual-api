using DiagnoseVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiagnoseVirtual.Infra.Data.Mapping
{
    public class UploadMonitoramentoMap : IEntityTypeConfiguration<UploadMonitoramento>
    {
        public void Configure(EntityTypeBuilder<UploadMonitoramento> builder)
        {
            //Tabela
            builder.ToTable("upload_monitoramento");
            builder.HasKey(x => x.Id);

            //Proriedades
            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.NomeArquivo)
                .IsRequired()
                .HasMaxLength(25)
                .HasColumnName("nome_arquivo");

            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("url");

            //Relacoes
            
            //Indices
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}