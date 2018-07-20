CREATE TABLE [dbo].[ProcedureStepHouse] (
    [Id]              INT IDENTITY (1, 1) NOT NULL,
    [ProcedureStepId] INT NOT NULL,
    [HouseId]         INT NOT NULL,
    CONSTRAINT [PK_ProcedureStepHouse] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureStepHouse_House] FOREIGN KEY ([HouseId]) REFERENCES [dbo].[House] ([Id]),
    CONSTRAINT [FK_ProcedureStepHouse_ProcedureStep] FOREIGN KEY ([ProcedureStepId]) REFERENCES [dbo].[ProcedureStep] ([Id]),
    CONSTRAINT [IX_ProcedureStepHouse] UNIQUE NONCLUSTERED ([ProcedureStepId] ASC, [HouseId] ASC)
);

