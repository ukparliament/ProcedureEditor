CREATE TABLE [dbo].[DeletedProcedureWorkPackagedThing] (
    [Id]                                INT                NOT NULL,
    [TripleStoreId]                     NVARCHAR (16)      NOT NULL,
    [ProcedureWorkPackageTripleStoreId] NVARCHAR (16)      NOT NULL,
    [DeletedAt]                         DATETIMEOFFSET (0) CONSTRAINT [DF_DeletedProcedureWorkPackagedThing_DeletedAt] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_DeletedProcedureWorkPackagedThing] PRIMARY KEY CLUSTERED ([Id] ASC)
);





