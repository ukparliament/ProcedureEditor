CREATE TABLE [dbo].[ProcedureWorkPackagedThingPreceding] (
    [Id]                         INT                IDENTITY (1, 1) NOT NULL,
    [WorkPackagedIsFollowedById] INT                NOT NULL,
    [WorkPackagedIsPrecededById] INT                NOT NULL,
    [ModifiedAt]                 DATETIMEOFFSET (0) NOT NULL,
    [ModifiedBy]                 NVARCHAR (MAX)     NOT NULL,
    CONSTRAINT [PK_ProcedureWorkPackagedThingPreceding] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureWorkPackagedThingPreceding_ProcedureWorkPackagedThing] FOREIGN KEY ([WorkPackagedIsFollowedById]) REFERENCES [dbo].[ProcedureWorkPackagedThing] ([Id]),
    CONSTRAINT [FK_ProcedureWorkPackagedThingPreceding_ProcedureWorkPackagedThing1] FOREIGN KEY ([WorkPackagedIsPrecededById]) REFERENCES [dbo].[ProcedureWorkPackagedThing] ([Id]),
    CONSTRAINT [IX_ProcedureWorkPackagedThingPreceding_1] UNIQUE NONCLUSTERED ([WorkPackagedIsFollowedById] ASC, [WorkPackagedIsPrecededById] ASC)
);










GO



GO





GO
CREATE TRIGGER [dbo].[OnDeleteProcedureWorkPackagedThingPreceding]
   ON [dbo].ProcedureWorkPackagedThingPreceding
   after DELETE
AS 
BEGIN
	SET NOCOUNT ON;

   insert into DeletedProcedureWorkPackagedThingPreceding (Id, FollowedByTripleStoreId, PrecededByTripleStoreId)
   select d.Id, fwt.TripleStoreId, pwt.TripleStoreId from deleted d
   join ProcedureWorkPackagedThing fwt on fwt.Id=d.WorkPackagedIsFollowedById
   join ProcedureWorkPackagedThing pwt on pwt.Id=d.WorkPackagedIsPrecededById

END