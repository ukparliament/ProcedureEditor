CREATE TABLE [dbo].[DeletedProcedureRoute] (
    [Id]            INT                NOT NULL,
    [TripleStoreId] NVARCHAR (16)      NOT NULL,
    [DeletedAt]     DATETIMEOFFSET (0) CONSTRAINT [DF_DeletedProcedureRoute_DeletedAt] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_DeletedProcedureRoute] PRIMARY KEY CLUSTERED ([Id] ASC)
);

