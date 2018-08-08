CREATE TABLE [dbo].[DeletedProcedureWorkPackagedThingPreceding] (
    [Id]                      INT                NOT NULL,
    [FollowedByTripleStoreId] NVARCHAR (16)      NOT NULL,
    [PrecededByTripleStoreId] NVARCHAR (16)      NOT NULL,
    [DeletedAt]               DATETIMEOFFSET (0) CONSTRAINT [DF_ProcedureWorkPackagedThingPreceding_DeletedAt] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_DeletedProcedureWorkPackagedThingPreceding] PRIMARY KEY CLUSTERED ([Id] ASC)
);

