CREATE TABLE [dbo].[Procedure] (
    [Id]            INT                IDENTITY (1, 1) NOT NULL,
    [TripleStoreId] NVARCHAR (16)      NOT NULL,
    [ProcedureName] NVARCHAR (MAX)     NOT NULL,
    [ModifiedBy]    NVARCHAR (MAX)     NOT NULL,
    [IsDeleted]     BIT                DEFAULT ((0)) NOT NULL,
    [ModifiedAt]    DATETIMEOFFSET (0) NOT NULL,
    CONSTRAINT [PK_Procedure] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Procedure] UNIQUE NONCLUSTERED ([TripleStoreId] ASC)
);

