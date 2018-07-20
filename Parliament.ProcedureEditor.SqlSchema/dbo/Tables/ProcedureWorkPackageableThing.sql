CREATE TABLE [dbo].[ProcedureWorkPackageableThing] (
    [Id]                                  INT                IDENTITY (1, 1) NOT NULL,
    [TripleStoreId]                       NVARCHAR (16)      NOT NULL,
    [ProcedureWorkPackageableThingName]   NVARCHAR (MAX)     NOT NULL,
    [StatutoryInstrumentNumber]           NVARCHAR (32)      NULL,
    [StatutoryInstrumentNumberPrefix]     NVARCHAR (32)      NULL,
    [StatutoryInstrumentNumberYear]       INT                NULL,
    [ComingIntoForceNote]                 NVARCHAR (MAX)     NULL,
    [WebLink]                             NVARCHAR (MAX)     NULL,
    [ProcedureWorkPackageableThingTypeId] INT                NULL,
    [ComingIntoForceDate]                 DATETIMEOFFSET (0) NULL,
    [TimeLimitForObjectionEndDate]        DATETIMEOFFSET (0) NULL,
    [ProcedureWorkPackageTripleStoreId]   NVARCHAR (16)      NOT NULL,
    [ProcedureId]                         INT                NOT NULL,
    [ModifiedAt]                          DATETIMEOFFSET (0) NOT NULL,
    [ModifiedBy]                          NVARCHAR (MAX)     NOT NULL,
    [IsDeleted]                           BIT                CONSTRAINT [DF__Procedure__IsDel__14270015] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProcedureWorkPackageableThing] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureWorkPackageableThing_Procedure] FOREIGN KEY ([ProcedureId]) REFERENCES [dbo].[Procedure] ([Id]),
    CONSTRAINT [FK_ProcedureWorkPackageableThing_ProcedureWorkPackageableThingType] FOREIGN KEY ([ProcedureWorkPackageableThingTypeId]) REFERENCES [dbo].[ProcedureWorkPackageableThingType] ([Id]),
    CONSTRAINT [IX_ProcedureWorkPackageableThing] UNIQUE NONCLUSTERED ([TripleStoreId] ASC),
    CONSTRAINT [IX_ProcedureWorkPackageableThing_1] UNIQUE NONCLUSTERED ([ProcedureWorkPackageTripleStoreId] ASC)
);

