CREATE TABLE [dbo].[SolrStatutoryInstrumentData] (
    [Id]                    INT                IDENTITY (1, 1) NOT NULL,
    [Uri]                   NVARCHAR (128)     NOT NULL,
    [SIPrefix]              NVARCHAR (32)      NULL,
    [SINumber]              INT                NULL,
    [WebUrl]                NVARCHAR (MAX)     NULL,
    [ComingIntoForceDate]   DATETIMEOFFSET (0) NULL,
    [MadeDate]              DATETIMEOFFSET (0) NULL,
    [Title]                 NVARCHAR (MAX)     NULL,
    [SIProcedure]           NVARCHAR (50)      NULL,
    [ComingIntoForceNote]   NVARCHAR (MAX)     NULL,
    [IsStatutoryInstrument] BIT                NULL,
    [TripleStoreId]         NVARCHAR (16)      NULL,
    [IsDeleted]             BIT                CONSTRAINT [DF_SolrStatutoryInstrumentData_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SolrStatutoryInstrumentData] PRIMARY KEY CLUSTERED ([Id] ASC)
);





