CREATE TABLE [dbo].[ProcedureBusinessItem] (
    [Id]                     INT                IDENTITY (1, 1) NOT NULL,
    [TripleStoreId]          NVARCHAR (16)      NOT NULL,
    [WebLink]                NVARCHAR (MAX)     NULL,
    [LayingBodyId]           INT                NULL,
    [ProcedureWorkPackageId] INT                NOT NULL,
    [ModifiedBy]             NVARCHAR (MAX)     NOT NULL,
    [IsDeleted]              BIT                CONSTRAINT [DF__Procedure__IsDel__123EB7A3] DEFAULT ((0)) NOT NULL,
    [ModifiedAt]             DATETIMEOFFSET (0) NOT NULL,
    [BusinessItemDate]       DATETIMEOFFSET (0) NULL,
    CONSTRAINT [PK_ProcedureBusinessItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureBusinessItem_AnsweringBody] FOREIGN KEY ([LayingBodyId]) REFERENCES [dbo].[LayingBody] ([Id]),
    CONSTRAINT [FK_ProcedureBusinessItem_ProcedureWorkPackageableThing] FOREIGN KEY ([ProcedureWorkPackageId]) REFERENCES [dbo].[ProcedureWorkPackageableThing] ([Id]),
    CONSTRAINT [IX_ProcedureBusinessItem] UNIQUE NONCLUSTERED ([TripleStoreId] ASC)
);

