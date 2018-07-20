CREATE TABLE [dbo].[ProcedureBusinessItemProcedureStep] (
    [Id]                      INT IDENTITY (1, 1) NOT NULL,
    [ProcedureBusinessItemId] INT NOT NULL,
    [ProcedureStepId]         INT NOT NULL,
    CONSTRAINT [PK_ProcedureBusinessItemProcedureStep] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureBusinessItemProcedureStep_ProcedureBusinessItem] FOREIGN KEY ([ProcedureBusinessItemId]) REFERENCES [dbo].[ProcedureBusinessItem] ([Id]),
    CONSTRAINT [FK_ProcedureBusinessItemProcedureStep_ProcedureStep] FOREIGN KEY ([ProcedureStepId]) REFERENCES [dbo].[ProcedureStep] ([Id]),
    CONSTRAINT [IX_ProcedureBusinessItemProcedureStep] UNIQUE NONCLUSTERED ([ProcedureBusinessItemId] ASC, [ProcedureStepId] ASC)
);

