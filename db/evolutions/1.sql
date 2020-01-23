﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE EXTENSION postgis;

CREATE SCHEMA IF NOT EXISTS diagnose_virtual;

CREATE TABLE diagnose_virtual.usuario (
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    nome character varying(100) NOT NULL,
    cpf character varying(11) NOT NULL,
    email character varying(50) NOT NULL,
    password_hash bytea NOT NULL,
    password_salt bytea NOT NULL,
    ativo boolean NOT NULL DEFAULT TRUE,
    CONSTRAINT "PK_usuario" PRIMARY KEY (id)
);

CREATE TABLE diagnose_virtual.fazenda (
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    demarcacao_geom geometry NULL,
    concluida boolean NOT NULL DEFAULT FALSE,
    ativa boolean NOT NULL DEFAULT TRUE,
    id_usuario integer NOT NULL,
    CONSTRAINT "PK_fazenda" PRIMARY KEY (id),
    CONSTRAINT "FK_fazenda_usuario_id_usuario" FOREIGN KEY (id_usuario) REFERENCES diagnose_virtual.usuario (id) ON DELETE CASCADE
);

CREATE TABLE diagnose_virtual.dados_fazenda (
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    cultura character varying(100) NOT NULL,
    area_total double precision NOT NULL,
    quantidade_lavouras integer NOT NULL,
    "IdFazenda" integer NOT NULL,
    CONSTRAINT "PK_dados_fazenda" PRIMARY KEY (id),
    CONSTRAINT "FK_dados_fazenda_fazenda_IdFazenda" FOREIGN KEY ("IdFazenda") REFERENCES diagnose_virtual.fazenda (id) ON DELETE CASCADE
);

CREATE TABLE diagnose_virtual.lavoura (
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    demarcacao_geom geometry NOT NULL,
    concluida boolean NOT NULL DEFAULT FALSE,
    "IdFazenda" integer NOT NULL,
    CONSTRAINT "PK_lavoura" PRIMARY KEY (id),
    CONSTRAINT "FK_lavoura_fazenda_IdFazenda" FOREIGN KEY ("IdFazenda") REFERENCES diagnose_virtual.fazenda (id) ON DELETE CASCADE
);

CREATE TABLE diagnose_virtual.localizacao_fazenda (
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    nome character varying(30) NOT NULL,
    estado character varying(20) NOT NULL,
    municipio character varying(50) NOT NULL,
    proprietario character varying(50) NOT NULL,
    gerente character varying(30) NOT NULL,
    contato character varying(50) NOT NULL,
    ponto_referencia character varying(70) NOT NULL,
    "IdFazenda" integer NOT NULL,
    CONSTRAINT "PK_localizacao_fazenda" PRIMARY KEY (id),
    CONSTRAINT "FK_localizacao_fazenda_fazenda_IdFazenda" FOREIGN KEY ("IdFazenda") REFERENCES diagnose_virtual.fazenda (id) ON DELETE CASCADE
);

CREATE TABLE diagnose_virtual.dados_lavoura (
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    nome character varying(20) NOT NULL,
    mes_ano_plantio character varying(7) NOT NULL,
    cultivar character varying(30) NOT NULL,
    numero_plantas integer NOT NULL,
    expacamento_vertical double precision NOT NULL,
    espacamento_horizontal double precision NOT NULL,
    observacoes character varying(250) NOT NULL,
    "IdLavoura" integer NOT NULL,
    CONSTRAINT "PK_dados_lavoura" PRIMARY KEY (id),
    CONSTRAINT "FK_dados_lavoura_lavoura_IdLavoura" FOREIGN KEY ("IdLavoura") REFERENCES diagnose_virtual.lavoura (id) ON DELETE CASCADE
);

CREATE TABLE diagnose_virtual.talhao (
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    geometria_geom geometry NOT NULL,
    "IdLavoura" integer NOT NULL,
    CONSTRAINT "PK_talhao" PRIMARY KEY (id),
    CONSTRAINT "FK_talhao_lavoura_IdLavoura" FOREIGN KEY ("IdLavoura") REFERENCES diagnose_virtual.lavoura (id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_dados_fazenda_id" ON diagnose_virtual.dados_fazenda (id);

CREATE UNIQUE INDEX "IX_dados_fazenda_IdFazenda" ON diagnose_virtual.dados_fazenda ("IdFazenda");

CREATE UNIQUE INDEX "IX_dados_lavoura_id" ON diagnose_virtual.dados_lavoura (id);

CREATE UNIQUE INDEX "IX_dados_lavoura_IdLavoura" ON diagnose_virtual.dados_lavoura ("IdLavoura");

CREATE UNIQUE INDEX "IX_fazenda_id" ON diagnose_virtual.fazenda (id);

CREATE INDEX "IX_fazenda_id_usuario" ON diagnose_virtual.fazenda (id_usuario);

CREATE UNIQUE INDEX "IX_lavoura_id" ON diagnose_virtual.lavoura (id);

CREATE INDEX "IX_lavoura_IdFazenda" ON diagnose_virtual.lavoura ("IdFazenda");

CREATE UNIQUE INDEX "IX_localizacao_fazenda_id" ON diagnose_virtual.localizacao_fazenda (id);

CREATE UNIQUE INDEX "IX_localizacao_fazenda_IdFazenda" ON diagnose_virtual.localizacao_fazenda ("IdFazenda");

CREATE UNIQUE INDEX "IX_talhao_id" ON diagnose_virtual.talhao (id);

CREATE INDEX "IX_talhao_IdLavoura" ON diagnose_virtual.talhao ("IdLavoura");

CREATE UNIQUE INDEX "IX_usuario_cpf" ON diagnose_virtual.usuario (cpf);

CREATE UNIQUE INDEX "IX_usuario_email" ON diagnose_virtual.usuario (email);

CREATE UNIQUE INDEX "IX_usuario_id" ON diagnose_virtual.usuario (id);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20200123012946_0', '3.1.1');

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20200123014804_1', '3.1.1');

