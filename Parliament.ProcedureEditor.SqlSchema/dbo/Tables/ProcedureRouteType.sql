CREATE TABLE [dbo].[ProcedureRouteType] (
    [Id]                     INT            IDENTITY (1, 1) NOT NULL,
    [ProcedureRouteTypeName] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProcedureRouteType] PRIMARY KEY CLUSTERED ([Id] ASC)
);



