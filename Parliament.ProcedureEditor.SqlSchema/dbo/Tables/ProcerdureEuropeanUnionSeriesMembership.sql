CREATE TABLE [dbo].[ProcerdureEuropeanUnionSeriesMembership] (
    [Id]                INT NOT NULL,
    [ProcedureTreatyId] INT NOT NULL,
    CONSTRAINT [PK_ProcerdureEuropeanUnionSeriesMembership] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcerdureEuropeanUnionSeriesMembership_ProcedureSeriesMembership] FOREIGN KEY ([Id]) REFERENCES [dbo].[ProcedureSeriesMembership] ([Id]),
    CONSTRAINT [FK_ProcerdureEuropeanUnionSeriesMembership_ProcedureTreaty] FOREIGN KEY ([ProcedureTreatyId]) REFERENCES [dbo].[ProcedureTreaty] ([Id]),
    CONSTRAINT [IX_ProcerdureEuropeanUnionSeriesMembership] UNIQUE NONCLUSTERED ([Id] ASC, [ProcedureTreatyId] ASC)
);

