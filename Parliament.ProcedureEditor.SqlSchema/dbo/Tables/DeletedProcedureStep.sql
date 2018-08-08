CREATE TABLE [dbo].[DeletedProcedureStep] (
    [Id]            INT                NOT NULL,
    [TripleStoreId] NVARCHAR (16)      NOT NULL,
    [DeletedAt]     DATETIMEOFFSET (0) CONSTRAINT [DF_DeletedProcedureStep_DeletedAt] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_DeletedProcedureStep] PRIMARY KEY CLUSTERED ([Id] ASC)
);

