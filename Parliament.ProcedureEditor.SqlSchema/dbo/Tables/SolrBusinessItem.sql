CREATE TABLE [dbo].[SolrBusinessItem] (
    [Id]                            INT           IDENTITY (1, 1) NOT NULL,
    [SolrStatutoryInstrumentDataId] INT           NOT NULL,
    [TripleStoreId]                 NVARCHAR (18) NOT NULL,
    CONSTRAINT [PK_SolrBusinessItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SolrBusinessItem_SolrStatutoryInstrumentData] FOREIGN KEY ([SolrStatutoryInstrumentDataId]) REFERENCES [dbo].[SolrStatutoryInstrumentData] ([Id]),
    CONSTRAINT [IX_SolrBusinessItem] UNIQUE NONCLUSTERED ([TripleStoreId] ASC)
);

