USE [master]

DECLARE @dbname nvarchar(128)
SET @dbname = N'VaultDbTest'
IF (EXISTS (SELECT [name] FROM sys.databases WHERE ([name] = @dbname OR [name] = @dbname)))
BEGIN
	DROP DATABASE [VaultDbTest]
END

CREATE DATABASE [VaultDbTest]