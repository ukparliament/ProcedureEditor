CREATE TABLE [dbo].[ProcerdureMiscellaneousSeriesMembership] (
    [Id]                INT NOT NULL,
    [ProcedureTreatyId] INT NOT NULL,
    CONSTRAINT [PK_ProcerdureMiscellaneousSeriesMembership] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcerdureMiscellaneousSeriesMembership_ProcedureSeriesMembership] FOREIGN KEY ([Id]) REFERENCES [dbo].[ProcedureSeriesMembership] ([Id]),
    CONSTRAINT [FK_ProcerdureMiscellaneousSeriesMembership_ProcedureTreaty] FOREIGN KEY ([ProcedureTreatyId]) REFERENCES [dbo].[ProcedureTreaty] ([Id]),
    CONSTRAINT [IX_ProcerdureMiscellaneousSeriesMembership] UNIQUE NONCLUSTERED ([Id] ASC, [ProcedureTreatyId] ASC)
);

