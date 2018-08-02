CREATE TABLE [dbo].[ProcedureStatutoryInstrument] (
    [Id]                               INT                NOT NULL,
    [ProcedureStatutoryInstrumentName] NVARCHAR (MAX)     NOT NULL,
    [StatutoryInstrumentNumber]        INT                NULL,
    [StatutoryInstrumentNumberPrefix]  NVARCHAR (32)      NULL,
    [StatutoryInstrumentNumberYear]    INT                NULL,
    [ComingIntoForceNote]              NVARCHAR (MAX)     NULL,
    [ComingIntoForceDate]              DATETIMEOFFSET (0) NULL,
    [MadeDate]                         DATETIMEOFFSET (0) NULL,
    CONSTRAINT [PK_ProcedureStatutoryInstrument] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureStatutoryInstrument_ProcedureWorkPackageableThing] FOREIGN KEY ([Id]) REFERENCES [dbo].[ProcedureWorkPackagedThing] ([Id])
);



