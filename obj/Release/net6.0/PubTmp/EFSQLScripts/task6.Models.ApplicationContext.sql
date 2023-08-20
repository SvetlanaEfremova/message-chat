IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230815122632_InitialCreate')
BEGIN
    CREATE TABLE [Messages] (
        [Id] uniqueidentifier NOT NULL,
        [Text] nvarchar(max) NOT NULL,
        [Tags] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Messages] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230815122632_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230815122632_InitialCreate', N'7.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818103038_AddedTagAndMessageTagModels')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Messages]') AND [c].[name] = N'Tags');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Messages] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Messages] DROP COLUMN [Tags];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818103038_AddedTagAndMessageTagModels')
BEGIN
    CREATE TABLE [Tags] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Tags] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818103038_AddedTagAndMessageTagModels')
BEGIN
    CREATE TABLE [MessageTag] (
        [MessageId] uniqueidentifier NOT NULL,
        [TagId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_MessageTag] PRIMARY KEY ([MessageId], [TagId]),
        CONSTRAINT [FK_MessageTag_Messages_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [Messages] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_MessageTag_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818103038_AddedTagAndMessageTagModels')
BEGIN
    CREATE INDEX [IX_MessageTag_TagId] ON [MessageTag] ([TagId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818103038_AddedTagAndMessageTagModels')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230818103038_AddedTagAndMessageTagModels', N'7.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818125321_DbSetForMessageTagsAdded')
BEGIN
    ALTER TABLE [MessageTag] DROP CONSTRAINT [FK_MessageTag_Messages_MessageId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818125321_DbSetForMessageTagsAdded')
BEGIN
    ALTER TABLE [MessageTag] DROP CONSTRAINT [FK_MessageTag_Tags_TagId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818125321_DbSetForMessageTagsAdded')
BEGIN
    ALTER TABLE [MessageTag] DROP CONSTRAINT [PK_MessageTag];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818125321_DbSetForMessageTagsAdded')
BEGIN
    EXEC sp_rename N'[MessageTag]', N'MessageTags';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818125321_DbSetForMessageTagsAdded')
BEGIN
    EXEC sp_rename N'[MessageTags].[IX_MessageTag_TagId]', N'IX_MessageTags_TagId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818125321_DbSetForMessageTagsAdded')
BEGIN
    ALTER TABLE [MessageTags] ADD CONSTRAINT [PK_MessageTags] PRIMARY KEY ([MessageId], [TagId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818125321_DbSetForMessageTagsAdded')
BEGIN
    ALTER TABLE [MessageTags] ADD CONSTRAINT [FK_MessageTags_Messages_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [Messages] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818125321_DbSetForMessageTagsAdded')
BEGIN
    ALTER TABLE [MessageTags] ADD CONSTRAINT [FK_MessageTags_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230818125321_DbSetForMessageTagsAdded')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230818125321_DbSetForMessageTagsAdded', N'7.0.10');
END;
GO

COMMIT;
GO

