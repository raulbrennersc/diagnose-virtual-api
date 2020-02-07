ALTER TABLE diagnose_virtual.dados_lavoura ALTER COLUMN mes_ano_plantio TYPE character varying(20);
ALTER TABLE diagnose_virtual.dados_lavoura ALTER COLUMN mes_ano_plantio SET NOT NULL;
ALTER TABLE diagnose_virtual.dados_lavoura ALTER COLUMN mes_ano_plantio DROP DEFAULT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20200131000751_3', '3.1.1');

