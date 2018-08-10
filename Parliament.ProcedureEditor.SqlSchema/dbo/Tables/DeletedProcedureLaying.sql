CREATE TABLE [dbo].[DeletedProcedureLaying] (
    [Id]            INT                NOT NULL,
    [TripleStoreId] NVARCHAR (16)      NOT NULL,
    [DeletedAt]     DATETIMEOFFSET (0) CONSTRAINT [DF_DeletedProcedureLaying_DeletedAt] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_DeletedProcedureLaying] PRIMARY KEY CLUSTERED ([Id] ASC)
);



