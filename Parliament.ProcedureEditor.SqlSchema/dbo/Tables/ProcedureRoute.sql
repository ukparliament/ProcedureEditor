CREATE TABLE [dbo].[ProcedureRoute] (
    [Id]                   INT                IDENTITY (1, 1) NOT NULL,
    [TripleStoreId]        NVARCHAR (16)      NOT NULL,
    [FromProcedureStepId]  INT                NOT NULL,
    [ToProcedureStepId]    INT                NOT NULL,
    [ProcedureRouteTypeId] INT                NOT NULL,
    [ModifiedBy]           NVARCHAR (MAX)     NOT NULL,
    [ModifiedAt]           DATETIMEOFFSET (0) NOT NULL,
    CONSTRAINT [PK_ProcedureRoute] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureRoute_ProcedureRouteType] FOREIGN KEY ([ProcedureRouteTypeId]) REFERENCES [dbo].[ProcedureRouteType] ([Id]),
    CONSTRAINT [FK_ProcedureRoute_ProcedureStep] FOREIGN KEY ([FromProcedureStepId]) REFERENCES [dbo].[ProcedureStep] ([Id]),
    CONSTRAINT [FK_ProcedureRoute_ProcedureStep1] FOREIGN KEY ([ToProcedureStepId]) REFERENCES [dbo].[ProcedureStep] ([Id]),
    CONSTRAINT [IX_ProcedureRoute] UNIQUE NONCLUSTERED ([TripleStoreId] ASC)
);








GO





GO
CREATE TRIGGER [dbo].[OnDeleteProcedureRoute]
   ON [dbo].ProcedureRoute
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

   insert into DeletedProcedureRoute (Id, TripleStoreId)
   select d.Id, d.TripleStoreId from deleted d

END