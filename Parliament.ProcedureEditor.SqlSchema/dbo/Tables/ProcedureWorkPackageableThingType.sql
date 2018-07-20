CREATE TABLE [dbo].[ProcedureWorkPackageableThingType] (
    [Id]                                    INT            IDENTITY (1, 1) NOT NULL,
    [ProcedureWorkPackageableThingTypeName] NVARCHAR (MAX) NOT NULL,
    [IsDeleted]                             BIT            CONSTRAINT [DF_ProcedureWorkPackageableThingType_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProcedureWorkPackageableThingType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

