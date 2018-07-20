CREATE TABLE [dbo].[ProcedureWorkPackageableThingPreceding] (
    [Id]                                       INT            IDENTITY (1, 1) NOT NULL,
    [PrecedingProcedureWorkPackageableThingId] INT            NOT NULL,
    [FollowingProcedureWorkPackageableThingId] INT            NOT NULL,
    [ModifiedAt]                               DATE           NOT NULL,
    [ModifiedBy]                               NVARCHAR (MAX) NOT NULL,
    [IsDeleted]                                BIT            CONSTRAINT [DF_ProcedureWorkPackageableThingPreceding_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProcedureWorkPackageableThingPreceding] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureWorkPackageableThingPreceding_ProcedureWorkPackageableThing] FOREIGN KEY ([PrecedingProcedureWorkPackageableThingId]) REFERENCES [dbo].[ProcedureWorkPackageableThing] ([Id]),
    CONSTRAINT [FK_ProcedureWorkPackageableThingPreceding_ProcedureWorkPackageableThing1] FOREIGN KEY ([FollowingProcedureWorkPackageableThingId]) REFERENCES [dbo].[ProcedureWorkPackageableThing] ([Id])
);

