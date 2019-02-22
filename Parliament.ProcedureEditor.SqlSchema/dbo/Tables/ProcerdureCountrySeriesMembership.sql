CREATE TABLE [dbo].[ProcerdureCountrySeriesMembership] (
    [Id]                INT NOT NULL,
    [ProcedureTreatyId] INT NOT NULL,
    CONSTRAINT [PK_ProcerdureCountrySeriesMembership] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcerdureCountrySeriesMembership_ProcedureSeriesMembership] FOREIGN KEY ([Id]) REFERENCES [dbo].[ProcedureSeriesMembership] ([Id]),
    CONSTRAINT [FK_ProcerdureCountrySeriesMembership_ProcedureTreaty] FOREIGN KEY ([ProcedureTreatyId]) REFERENCES [dbo].[ProcedureTreaty] ([Id]),
    CONSTRAINT [IX_ProcerdureCountrySeriesMembership] UNIQUE NONCLUSTERED ([Id] ASC, [ProcedureTreatyId] ASC)
);

