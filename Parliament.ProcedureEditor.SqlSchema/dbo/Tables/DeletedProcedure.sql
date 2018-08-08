CREATE TABLE [dbo].[DeletedProcedure] (
    [Id]            INT                NOT NULL,
    [TripleStoreId] NVARCHAR (16)      NOT NULL,
    [DeletedAt]     DATETIMEOFFSET (0) CONSTRAINT [DF_DeletedProcedure_DeletedAt] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_DeletedProcedure] PRIMARY KEY CLUSTERED ([Id] ASC)
);

