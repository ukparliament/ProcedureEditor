CREATE TABLE [dbo].[SolrTreatyBusinessItem] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [SolrTreatyDataId] INT           NOT NULL,
    [TripleStoreId]    NVARCHAR (16) NOT NULL,
    CONSTRAINT [PK_SolrTreatyBusinessItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SolrTreatyBusinessItem_SolrTreatyData] FOREIGN KEY ([SolrTreatyDataId]) REFERENCES [dbo].[SolrTreatyData] ([Id]),
    CONSTRAINT [IX_SolrTreatyBusinessItem] UNIQUE NONCLUSTERED ([TripleStoreId] ASC)
);

