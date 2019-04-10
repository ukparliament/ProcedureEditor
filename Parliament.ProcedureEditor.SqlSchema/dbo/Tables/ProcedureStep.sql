CREATE TABLE [dbo].[ProcedureStep] (
    [Id]                                         INT                IDENTITY (1, 1) NOT NULL,
    [TripleStoreId]                              NVARCHAR (16)      NOT NULL,
    [ProcedureStepName]                          NVARCHAR (MAX)     NOT NULL,
    [ModifiedBy]                                 NVARCHAR (MAX)     NOT NULL,
    [ModifiedAt]                                 DATETIMEOFFSET (0) NOT NULL,
    [ProcedureStepDescription]                   NVARCHAR (MAX)     NULL,
    [ProcedureStepScopeNote]                     NVARCHAR (MAX)     NULL,
    [ProcedureStepLinkNote]                      NVARCHAR (MAX)     NULL,
    [ProcedureStepDateNote]                      NVARCHAR (MAX)     NULL,
    [CommonlyActualisedAlongsideProcedureStepId] INT                NULL,
    [ProcedureStepPublicationId]                 INT                NULL,
    CONSTRAINT [PK_ProcedureStep] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureStep_ProcedureStep] FOREIGN KEY ([CommonlyActualisedAlongsideProcedureStepId]) REFERENCES [dbo].[ProcedureStep] ([Id]),
    CONSTRAINT [FK_ProcedureStep_ProcedureStepPublication] FOREIGN KEY ([ProcedureStepPublicationId]) REFERENCES [dbo].[ProcedureStepPublication] ([Id]),
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