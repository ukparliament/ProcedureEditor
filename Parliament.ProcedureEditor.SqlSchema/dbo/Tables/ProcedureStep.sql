CREATE TABLE [dbo].[ProcedureStep] (
    [Id]                       INT                IDENTITY (1, 1) NOT NULL,
    [TripleStoreId]            NVARCHAR (16)      NOT NULL,
    [ProcedureStepName]        NVARCHAR (MAX)     NOT NULL,
    [ModifiedBy]               NVARCHAR (MAX)     NOT NULL,
    [ModifiedAt]               DATETIMEOFFSET (0) NOT NULL,
    [ProcedureStepDescription] NVARCHAR (MAX)     NULL,
    CONSTRAINT [PK_ProcedureStep] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_ProcedureStep] UNIQUE NONCLUSTERED ([TripleStoreId] ASC)
);




GO
CREATE TRIGGER [dbo].[OnDeleteProcedureStep]
   ON [dbo].ProcedureStep
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

   insert into DeletedProcedureStep (Id, TripleStoreId)
   select d.Id, d.TripleStoreId from deleted d

END