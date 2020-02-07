ALTER TABLE diagnose_virtual.lavoura ALTER COLUMN demarcacao_geom TYPE geometry;
ALTER TABLE diagnose_virtual.lavoura ALTER COLUMN demarcacao_geom DROP NOT NULL;
ALTER TABLE diagnose_virtual.lavoura ALTER COLUMN demarcacao_geom DROP DEFAULT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20200128224018_2', '3.1.1');

