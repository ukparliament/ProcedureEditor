CREATE TABLE [dbo].[ProcedureWorkPackagedThing] (
    [Id]                                INT                IDENTITY (1, 1) NOT NULL,
    [TripleStoreId]                     NVARCHAR (16)      NOT NULL,
    [WebLink]                           NVARCHAR (MAX)     NULL,
    [ProcedureWorkPackageTripleStoreId] NVARCHAR (16)      NOT NULL,
    [ProcedureId]                       INT                NOT NULL,
    [ModifiedAt]                        DATETIMEOFFSET (0) NOT NULL,
    [ModifiedBy]                        NVARCHAR (MAX)     NOT NULL,
    [IsDeleted]                         BIT                CONSTRAINT [DF_ProcedureWorkPackagedThing_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProcedureWorkPackagedThing] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureWorkPackagedThing_Procedure] FOREIGN KEY ([ProcedureId]) REFERENCES [dbo].[Procedure] ([Id]),
    CONSTRAINT [IX_ProcedureWorkPackagedThing] UNIQUE NONCLUSTERED ([TripleStoreId] ASC),
    CONSTRAINT [IX_ProcedureWorkPackagedThing_1] UNIQUE NONCLUSTERED ([ProcedureWorkPackageTripleStoreId] ASC)
);



