CREATE TABLE [dbo].[ProcedureBusinessItem] (
    [Id]                     INT                IDENTITY (1, 1) NOT NULL,
    [TripleStoreId]          NVARCHAR (16)      NOT NULL,
    [WebLink]                NVARCHAR (MAX)     NULL,
    [ProcedureWorkPackageId] INT                NOT NULL,
    [ModifiedBy]             NVARCHAR (MAX)     NOT NULL,
    [ModifiedAt]             DATETIMEOFFSET (0) NOT NULL,
    [BusinessItemDate]       DATETIMEOFFSET (0) NULL,
    CONSTRAINT [PK_ProcedureBusinessItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureBusinessItem_ProcedureWorkPackageableThing] FOREIGN KEY ([ProcedureWorkPackageId]) REFERENCES [dbo].[ProcedureWorkPackagedThing] ([Id]),
    CONSTRAINT [IX_ProcedureBusinessItem] UNIQUE NONCLUSTERED ([TripleStoreId] ASC)
);










GO
CREATE TRIGGER [dbo].[OnDeleteProcedureBusinessItem]
   ON [dbo].ProcedureBusinessItem
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

   insert into DeletedProcedureBusinessItem (Id, TripleStoreId)
   select d.Id, d.TripleStoreId from deleted d

END