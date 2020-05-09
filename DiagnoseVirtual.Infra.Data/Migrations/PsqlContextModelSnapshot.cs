﻿// <auto-generated />
using System;
using DiagnoseVirtual.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DiagnoseVirtual.Infra.Data.Migrations
{
    [DbContext(typeof(PsqlContext))]
    partial class PsqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("diagnose_virtual")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.Cultura", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("nome")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("cultura");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.DadosFazenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("AreaTotal")
                        .HasColumnName("area_total")
                        .HasColumnType("double precision");

                    b.Property<int>("QuantidadeLavouras")
                        .HasColumnName("quantidade_lavouras")
                        .HasColumnType("integer");

                    b.Property<int?>("id_cultura")
                        .HasColumnType("integer");

                    b.Property<int?>("id_fazenda")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("id_cultura");

                    b.HasIndex("id_fazenda")
                        .IsUnique();

                    b.ToTable("dados_fazenda");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.DadosLavoura", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Cultivar")
                        .IsRequired()
                        .HasColumnName("cultivar")
                        .HasColumnType("character varying(30)")
                        .HasMaxLength(30);

                    b.Property<double>("EspacamentoHorizontal")
                        .HasColumnName("espacamento_horizontal")
                        .HasColumnType("double precision");

                    b.Property<double>("EspacamentoVertical")
                        .HasColumnName("expacamento_vertical")
                        .HasColumnType("double precision");

                    b.Property<string>("MesAnoPlantio")
                        .IsRequired()
                        .HasColumnName("mes_ano_plantio")
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("nome")
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<int>("NumeroPlantas")
                        .HasColumnName("numero_plantas")
                        .HasColumnType("integer");

                    b.Property<string>("Observacoes")
                        .IsRequired()
                        .HasColumnName("observacoes")
                        .HasColumnType("character varying(250)")
                        .HasMaxLength(250);

                    b.Property<int?>("id_lavoura")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("id_lavoura")
                        .IsUnique();

                    b.ToTable("dados_lavoura");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.Estado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("nome")
                        .HasColumnType("character varying(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Sigla")
                        .IsRequired()
                        .HasColumnName("sigla")
                        .HasColumnType("character varying(2)")
                        .HasMaxLength(2);

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("estado");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.EtapaFazenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("nome")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("etapa_fazenda");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.EtapaLavoura", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("nome")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("etapa_lavoura");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.Fazenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Ativa")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ativa")
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<bool>("Concluida")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("concluida")
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<Geometry>("Demarcacao")
                        .HasColumnName("demarcacao_geom")
                        .HasColumnType("geometry");

                    b.Property<int?>("id_etapa")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("id_usuario")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("id_etapa");

                    b.HasIndex("id_usuario");

                    b.ToTable("fazenda");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.Lavoura", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Concluida")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("concluida")
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<Geometry>("Demarcacao")
                        .HasColumnName("demarcacao_geom")
                        .HasColumnType("geometry");

                    b.Property<Geometry[]>("Talhoes")
                        .HasColumnName("talhoes")
                        .HasColumnType("geometry[]");

                    b.Property<int?>("id_etapa")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("id_fazenda")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("id_etapa");

                    b.HasIndex("id_fazenda");

                    b.ToTable("lavoura");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.LocalizacaoFazenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Gerente")
                        .IsRequired()
                        .HasColumnName("gerente")
                        .HasColumnType("character varying(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("nome")
                        .HasColumnType("character varying(30)")
                        .HasMaxLength(30);

                    b.Property<string>("PontoReferencia")
                        .IsRequired()
                        .HasColumnName("ponto_referencia")
                        .HasColumnType("character varying(70)")
                        .HasMaxLength(70);

                    b.Property<string>("Proprietario")
                        .IsRequired()
                        .HasColumnName("proprietario")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnName("telefone")
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<int?>("id_fazenda")
                        .HasColumnType("integer");

                    b.Property<int?>("id_municipio")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("id_fazenda")
                        .IsUnique();

                    b.HasIndex("id_municipio");

                    b.ToTable("localizacao_fazenda");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.Monitoramento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Ativo")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ativo")
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<DateTime>("DataMonitoramento")
                        .HasColumnName("data_monitoramento")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("id_fazenda")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("id_fazenda");

                    b.ToTable("monitoramento");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.Municipio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CodigoIbge")
                        .HasColumnName("codigo_ibge")
                        .HasColumnType("integer");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("nome")
                        .HasColumnType("text");

                    b.Property<int?>("id_estado")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("id_estado");

                    b.ToTable("municipio");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.ProblemaMonitoramento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnName("descricao")
                        .HasColumnType("character varying(140)")
                        .HasMaxLength(140);

                    b.Property<Point>("Ponto")
                        .IsRequired()
                        .HasColumnName("ponto_geom")
                        .HasColumnType("geometry");

                    b.Property<string>("Recomendacao")
                        .IsRequired()
                        .HasColumnName("recomendacao")
                        .HasColumnType("character varying(140)")
                        .HasMaxLength(140);

                    b.Property<int?>("id_monitoramento")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("id_monitoramento");

                    b.ToTable("problema_monitoramento");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.UploadMonitoramento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("NomeArquivo")
                        .IsRequired()
                        .HasColumnName("nome_arquivo")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnName("url")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<int?>("id_monitoramento")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("id_monitoramento");

                    b.ToTable("upload_monitoramento");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Ativo")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ativo")
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnName("cpf")
                        .HasColumnType("character varying(11)")
                        .HasMaxLength(11);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("nome")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnName("password_hash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnName("password_salt")
                        .HasColumnType("bytea");

                    b.Property<bool>("PrimeiroAcesso")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("primeiro_acesso")
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.HasKey("Id");

                    b.HasIndex("Cpf")
                        .IsUnique();

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("usuario");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.DadosFazenda", b =>
                {
                    b.HasOne("DiagnoseVirtual.Domain.Entities.Cultura", "Cultura")
                        .WithMany()
                        .HasForeignKey("id_cultura");

                    b.HasOne("DiagnoseVirtual.Domain.Entities.Fazenda", "Fazenda")
                        .WithOne("DadosFazenda")
                        .HasForeignKey("DiagnoseVirtual.Domain.Entities.DadosFazenda", "id_fazenda");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.DadosLavoura", b =>
                {
                    b.HasOne("DiagnoseVirtual.Domain.Entities.Lavoura", "Lavoura")
                        .WithOne("DadosLavoura")
                        .HasForeignKey("DiagnoseVirtual.Domain.Entities.DadosLavoura", "id_lavoura");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.Fazenda", b =>
                {
                    b.HasOne("DiagnoseVirtual.Domain.Entities.EtapaFazenda", "Etapa")
                        .WithMany()
                        .HasForeignKey("id_etapa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiagnoseVirtual.Domain.Entities.Usuario", "Usuario")
                        .WithMany("Fazendas")
                        .HasForeignKey("id_usuario");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.Lavoura", b =>
                {
                    b.HasOne("DiagnoseVirtual.Domain.Entities.EtapaLavoura", "Etapa")
                        .WithMany()
                        .HasForeignKey("id_etapa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiagnoseVirtual.Domain.Entities.Fazenda", "Fazenda")
                        .WithMany("Lavouras")
                        .HasForeignKey("id_fazenda");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.LocalizacaoFazenda", b =>
                {
                    b.HasOne("DiagnoseVirtual.Domain.Entities.Fazenda", "Fazenda")
                        .WithOne("LocalizacaoFazenda")
                        .HasForeignKey("DiagnoseVirtual.Domain.Entities.LocalizacaoFazenda", "id_fazenda");

                    b.HasOne("DiagnoseVirtual.Domain.Entities.Municipio", "Municipio")
                        .WithMany()
                        .HasForeignKey("id_municipio");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.Monitoramento", b =>
                {
                    b.HasOne("DiagnoseVirtual.Domain.Entities.Fazenda", "Fazenda")
                        .WithMany("Monitoramentos")
                        .HasForeignKey("id_fazenda");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.Municipio", b =>
                {
                    b.HasOne("DiagnoseVirtual.Domain.Entities.Estado", "Estado")
                        .WithMany("Municipios")
                        .HasForeignKey("id_estado");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.ProblemaMonitoramento", b =>
                {
                    b.HasOne("DiagnoseVirtual.Domain.Entities.Monitoramento", "Monitoramento")
                        .WithMany("Problemas")
                        .HasForeignKey("id_monitoramento");
                });

            modelBuilder.Entity("DiagnoseVirtual.Domain.Entities.UploadMonitoramento", b =>
                {
                    b.HasOne("DiagnoseVirtual.Domain.Entities.Monitoramento", "Monitoramento")
                        .WithMany("Uploads")
                        .HasForeignKey("id_monitoramento");
                });
#pragma warning restore 612, 618
        }
    }
}
