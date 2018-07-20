CREATE TABLE [dbo].[ProcedureRouteType] (
    [Id]                     INT            IDENTITY (1, 1) NOT NULL,
    [ProcedureRouteTypeName] NVARCHAR (MAX) NOT NULL,
    [IsDeleted]              BIT            CONSTRAINT [DF_ProcedureRouteType_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ProcedureRouteType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

