CREATE TABLE [dbo].[ProcedureProposedNegativeStatutoryInstrument] (
    [Id]                                               INT            NOT NULL,
    [ProcedureProposedNegativeStatutoryInstrumentName] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProcedureProposedNegativeStatutoryInstrument] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureProposedNegativeStatutoryInstrument_ProcedureWorkPackageableThing] FOREIGN KEY ([Id]) REFERENCES [dbo].[ProcedureWorkPackagedThing] ([Id])
);

