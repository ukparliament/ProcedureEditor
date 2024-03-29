﻿CREATE TABLE [dbo].[ProcedureTreaty] (
    [Id]                                      INT                NOT NULL,
    [ProcedureTreatyName]                     NVARCHAR (MAX)     NOT NULL,
    [TreatyNumber]                            INT                NULL,
    [TreatyPrefix]                            NVARCHAR (32)      NULL,
    [ComingIntoForceNote]                     NVARCHAR (MAX)     NULL,
    [ComingIntoForceDate]                     DATETIMEOFFSET (0) NULL,
    [LeadGovernmentOrganisationTripleStoreId] NVARCHAR (16)      NULL,
    CONSTRAINT [PK_ProcedureTreaty] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureTreaty_ProcedureWorkPackagedThing] FOREIGN KEY ([Id]) REFERENCES [dbo].[ProcedureWorkPackagedThing] ([Id])
);





