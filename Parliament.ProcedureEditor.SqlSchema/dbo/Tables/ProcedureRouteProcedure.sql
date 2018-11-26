CREATE TABLE [dbo].[ProcedureRouteProcedure] (
    [Id]               INT IDENTITY (1, 1) NOT NULL,
    [ProcedureRouteId] INT NOT NULL,
    [ProcedureId]      INT NOT NULL,
    CONSTRAINT [PK_ProcedureRouteProcedure] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureRouteProcedure_Procedure] FOREIGN KEY ([ProcedureId]) REFERENCES [dbo].[Procedure] ([Id]),
    CONSTRAINT [FK_ProcedureRouteProcedure_ProcedureRoute] FOREIGN KEY ([ProcedureRouteId]) REFERENCES [dbo].[ProcedureRoute] ([Id]),
    CONSTRAINT [IX_ProcedureRouteProcedure] UNIQUE NONCLUSTERED ([ProcedureRouteId] ASC, [ProcedureId] ASC)
);

