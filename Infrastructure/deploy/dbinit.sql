USE [master]
DECLARE @dbname nvarchar(128)
SET @dbname = N'VaultDb'

IF (NOT EXISTS (SELECT [name] FROM sys.databases WHERE ([name] = @dbname OR [name] = @dbname)))
BEGIN	
	CREATE DATABASE [VaultDb]
	PRINT 'Database VaultDb created'
END
GO

USE [VaultDb]
GO
IF NOT EXISTS(SELECT * FROM SYS.DATABASE_PRINCIPALS WHERE [Name] = 'VaultAppUser')
BEGIN
	CREATE LOGIN VaultAppUser WITH PASSWORD = '1234Test!'
	CREATE USER VaultAppUser FOR LOGIN VaultAppUser
END

GRANT SELECT, INSERT, UPDATE, DELETE, REFERENCES 
ON DATABASE :: [VaultDb]
TO VaultAppUser


IF(NOT EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'Users'))
BEGIN
	CREATE TABLE [Users]
	(
		[UserName] VARCHAR(500) NOT NULL PRIMARY KEY,
		[Password] VARCHAR(2000) NOT NULL,
		[CreatedAt] DATETIME,
		[CurrentStatus] INT
	)
END

IF(NOT EXISTS(SELECT 1 FROM sys.tables WHERE [name] = 'Secrets'))
BEGIN
	CREATE TABLE [Secrets]
	(
		[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT(NEWID()),
		[SecretName] VARCHAR(2000),
		[SecretValue] NVARCHAR NOT NULL,
		[CreatedAt] DATETIME NOT NULL,
		[Owner] VARCHAR(500) NOT NULL FOREIGN KEY ([Owner]) REFERENCES [Users]([UserName])
	)
END
GO
USE [Master]
GO