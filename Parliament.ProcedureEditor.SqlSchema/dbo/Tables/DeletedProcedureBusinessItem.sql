CREATE TABLE [dbo].[DeletedProcedureBusinessItem] (
    [Id]            INT                NOT NULL,
    [TripleStoreId] NVARCHAR (16)      NOT NULL,
    [DeletedAt]     DATETIMEOFFSET (0) CONSTRAINT [DF_DeletedProcedureBusinessItem_DeletedAt] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_DeletedProcedureBusinessItem] PRIMARY KEY CLUSTERED ([Id] ASC)
);

