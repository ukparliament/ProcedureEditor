CREATE TABLE [dbo].[SolrTreatyData] (
    [Id]            INT                IDENTITY (1, 1) NOT NULL,
    [Uri]           NVARCHAR (128)     NOT NULL,
    [Prefix]        NVARCHAR (32)      NULL,
    [Number]        INT                NULL,
    [WebUrl]        NVARCHAR (MAX)     NULL,
    [LaidDate]      DATETIMEOFFSET (0) NULL,
    [Title]         NVARCHAR (MAX)     NULL,
    [Series]        NVARCHAR (MAX)     NULL,
    [IsTreaty]      BIT                NULL,
    [TripleStoreId] NVARCHAR (16)      NULL,
    [IsDeleted]     BIT                CONSTRAINT [DF_SolrTreatyData_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SolrTreatyData] PRIMARY KEY CLUSTERED ([Id] ASC)
);





