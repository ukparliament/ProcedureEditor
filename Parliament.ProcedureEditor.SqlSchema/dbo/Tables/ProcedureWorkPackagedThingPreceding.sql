CREATE TABLE [dbo].[ProcedureWorkPackagedThingPreceding] (
    [Id]                                             INT            IDENTITY (1, 1) NOT NULL,
    [ProcedureProposedNegativeStatutoryInstrumentId] INT            NOT NULL,
    [ProcedureStatutoryInstrumentId]                 INT            NOT NULL,
    [ModifiedAt]                                     DATE           NOT NULL,
    [ModifiedBy]                                     NVARCHAR (MAX) NOT NULL,
    [IsDeleted]                                      BIT            CONSTRAINT [DF_ProcedureWorkPackagedThingPreceding_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProcedureWorkPackagedThingPreceding] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureWorkPackagedThingPreceding_ProcedureProposedNegativeStatutoryInstrument] FOREIGN KEY ([ProcedureProposedNegativeStatutoryInstrumentId]) REFERENCES [dbo].[ProcedureProposedNegativeStatutoryInstrument] ([Id]),
    CONSTRAINT [FK_ProcedureWorkPackagedThingPreceding_ProcedureStatutoryInstrument] FOREIGN KEY ([ProcedureStatutoryInstrumentId]) REFERENCES [dbo].[ProcedureStatutoryInstrument] ([Id])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProcedureWorkPackagedThingPreceding_1]
    ON [dbo].[ProcedureWorkPackagedThingPreceding]([ProcedureStatutoryInstrumentId] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProcedureWorkPackagedThingPreceding]
    ON [dbo].[ProcedureWorkPackagedThingPreceding]([ProcedureProposedNegativeStatutoryInstrumentId] ASC) WHERE ([IsDeleted]=(0));

