CREATE TABLE [dbo].[House] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [TripleStoreId] NVARCHAR (16)  NOT NULL,
    [HouseName]     NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_House] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_House] UNIQUE NONCLUSTERED ([TripleStoreId] ASC)
);

