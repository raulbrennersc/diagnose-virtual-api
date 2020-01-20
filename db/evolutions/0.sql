create database qipixel;
create extension postgis;
create schema diagnose_virtual;

create table diagnose_virtual.usuario(
id integer serial primary key,
cpf varchar(11) not null,
nome varchar(70) not null,
email varchar(50) not null,
password_hash bytea not null,
password_salt bytea not null
);