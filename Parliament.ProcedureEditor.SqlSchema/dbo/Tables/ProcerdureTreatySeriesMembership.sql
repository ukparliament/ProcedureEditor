CREATE TABLE [dbo].[ProcerdureTreatySeriesMembership] (
    [Id]                INT NOT NULL,
    [ProcedureTreatyId] INT NOT NULL,
    CONSTRAINT [PK_ProcerdureTreatySeriesMembership] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcerdureTreatySeriesMembership_ProcedureSeriesMembership] FOREIGN KEY ([Id]) REFERENCES [dbo].[ProcedureSeriesMembership] ([Id]),
    CONSTRAINT [FK_ProcerdureTreatySeriesMembership_ProcedureTreaty] FOREIGN KEY ([ProcedureTreatyId]) REFERENCES [dbo].[ProcedureTreaty] ([Id]),
    CONSTRAINT [IX_ProcerdureTreatySeriesMembership] UNIQUE NONCLUSTERED ([Id] ASC, [ProcedureTreatyId] ASC)
);

