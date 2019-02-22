CREATE TABLE [dbo].[ProcedureSeriesMembership] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Citation] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ProcedureSeriesMembership] PRIMARY KEY CLUSTERED ([Id] ASC)
);

