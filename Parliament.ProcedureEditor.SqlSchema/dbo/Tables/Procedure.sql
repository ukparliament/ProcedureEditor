CREATE TABLE [dbo].[Procedure] (
    [Id]                   INT                IDENTITY (1, 1) NOT NULL,
    [TripleStoreId]        NVARCHAR (16)      NOT NULL,
    [ProcedureName]        NVARCHAR (MAX)     NOT NULL,
    [ProcedureDescription] NVARCHAR (MAX)     NULL,
    [ModifiedBy]           NVARCHAR (MAX)     NOT NULL,
    [ModifiedAt]           DATETIMEOFFSET (0) NOT NULL,
    CONSTRAINT [PK_Procedure] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Procedure] UNIQUE NONCLUSTERED ([TripleStoreId] ASC)
);






GO
CREATE TRIGGER [dbo].[OnDeleteProcedure]
   ON [dbo].[Procedure]
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

   insert into DeletedProcedure (Id, TripleStoreId)
   select d.Id, d.TripleStoreId from deleted d

END