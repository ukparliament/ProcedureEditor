CREATE TABLE [dbo].[ProcedureLaying] (
    [Id]                      INT                IDENTITY (1, 1) NOT NULL,
    [ProcedureBusinessItemId] INT                NOT NULL,
    [ProcedureWorkPackagedId] INT                NOT NULL,
    [LayingDate]              DATETIMEOFFSET (7) NULL,
    [LayingBodyId]            INT                NULL,
    [PersonTripleStoreId]     NVARCHAR (16)      NULL,
    [ModifiedBy]              NVARCHAR (MAX)     NOT NULL,
    [ModifiedAt]              DATETIMEOFFSET (0) CONSTRAINT [DF_ProcedureLaying_ModifiedAt] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_ProcedureLaying] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureLaying_LayingBody] FOREIGN KEY ([LayingBodyId]) REFERENCES [dbo].[LayingBody] ([Id]),
    CONSTRAINT [FK_ProcedureLaying_ProcedureBusinessItem] FOREIGN KEY ([ProcedureBusinessItemId]) REFERENCES [dbo].[ProcedureBusinessItem] ([Id]),
    CONSTRAINT [FK_ProcedureLaying_ProcedureWorkPackagedThing] FOREIGN KEY ([ProcedureWorkPackagedId]) REFERENCES [dbo].[ProcedureWorkPackagedThing] ([Id])
);




GO
CREATE TRIGGER [dbo].[OnDeleteProcedureLaying]
   ON [dbo].[ProcedureLaying]
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

   insert into DeletedProcedureLaying (Id, TripleStoreId)
   select d.Id, bi.TripleStoreId from deleted d
   join ProcedureBusinessItem bi on bi.Id=d.ProcedureBusinessItemId

END