CREATE TABLE [dbo].[LayingBody] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [TripleStoreId]  NVARCHAR (16)  NOT NULL,
    [LayingBodyName] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_LayingBody] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_LayingBody] UNIQUE NONCLUSTERED ([TripleStoreId] ASC)
);



