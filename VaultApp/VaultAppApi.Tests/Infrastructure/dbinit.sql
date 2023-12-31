USE [VaultDbTest]


CREATE TABLE [Users]
(
	[UserName] VARCHAR(500) NOT NULL PRIMARY KEY,
	[Password] VARCHAR(2000) NOT NULL,
	[CreatedAt] DATETIME,
	[CurrentStatus] INT
)

CREATE TABLE [Secrets]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT(NEWID()),
	[SecretName] VARCHAR(1000) UNIQUE,
	[SecretValue] NVARCHAR(max) NOT NULL,
	[CreatedAt] DATETIME NOT NULL,
	[Owner] VARCHAR(500) NOT NULL FOREIGN KEY ([Owner]) REFERENCES [Users]([UserName])
)

USE [Master]
