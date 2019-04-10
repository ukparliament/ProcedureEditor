CREATE TABLE [dbo].[ProcedureStepPublication] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [TripleStoreId]   NVARCHAR (16)  NOT NULL,
    [PublicationName] NVARCHAR (MAX) NULL,
    [PublicationUrl]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ProcedureStepPublication] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_ProcedureStepPublication] UNIQUE NONCLUSTERED ([TripleStoreId] ASC)
);

