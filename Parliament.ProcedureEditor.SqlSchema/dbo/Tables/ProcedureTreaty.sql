CREATE TABLE [dbo].[ProcedureTreaty] (
    [Id]                  INT            NOT NULL,
    [ProcedureTreatyName] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProcedureTreaty] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureTreaty_ProcedureWorkPackagedThing] FOREIGN KEY ([Id]) REFERENCES [dbo].[ProcedureWorkPackagedThing] ([Id])
);

