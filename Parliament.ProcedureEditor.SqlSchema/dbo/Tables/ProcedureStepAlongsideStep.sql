CREATE TABLE [dbo].[ProcedureStepAlongsideStep] (
    [Id]                                         INT IDENTITY (1, 1) NOT NULL,
    [ProcedureStepId]                            INT NOT NULL,
    [CommonlyActualisedAlongsideProcedureStepId] INT NOT NULL,
    CONSTRAINT [PK_ProcedureStepAlongsideStep] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureStepAlongsideStep_ProcedureStep] FOREIGN KEY ([ProcedureStepId]) REFERENCES [dbo].[ProcedureStep] ([Id]),
    CONSTRAINT [FK_ProcedureStepAlongsideStep_ProcedureStep1] FOREIGN KEY ([CommonlyActualisedAlongsideProcedureStepId]) REFERENCES [dbo].[ProcedureStep] ([Id]),
    CONSTRAINT [IX_ProcedureStepAlongsideStep] UNIQUE NONCLUSTERED ([ProcedureStepId] ASC, [CommonlyActualisedAlongsideProcedureStepId] ASC)
);

