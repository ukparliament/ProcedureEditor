CREATE PROCEDURE [dbo].[ClearDatabaseBeforeImport-Obsolete]
AS
BEGIN
    SET NOCOUNT ON

	delete from ProcedureBusinessItemProcedureWorkPackage
	delete from ProcedureBusinessItemProcedureStep
	delete from ProcedureBusinessItem
	delete from AnsweringBody
	delete from ProcedureWorkPackageableThingPreceding
	delete from ProcedureWorkPackageableThing
	delete from ProcedureWorkPackageableThingType
	delete from ProcedureRoute
	delete from ProcedureRouteType
	delete from ProcedureStepHouse
	delete from ProcedureStep
	delete from [Procedure]

	DBCC CHECKIDENT (ProcedureBusinessItemProcedureWorkPackage, reseed, 0)
	DBCC CHECKIDENT (ProcedureBusinessItemProcedureStep, reseed, 0)
	DBCC CHECKIDENT (ProcedureBusinessItem, reseed, 0)
	DBCC CHECKIDENT (AnsweringBody, reseed, 0)
	DBCC CHECKIDENT (ProcedureWorkPackageableThingPreceding, reseed, 0)
	DBCC CHECKIDENT (ProcedureWorkPackageableThing, reseed, 0)
	DBCC CHECKIDENT (ProcedureWorkPackageableThingType, reseed, 0)
	DBCC CHECKIDENT (ProcedureRoute, reseed, 0)
	DBCC CHECKIDENT (ProcedureRouteType, reseed, 0)
	DBCC CHECKIDENT (ProcedureStepHouse, reseed, 0)
	DBCC CHECKIDENT (ProcedureStep, reseed, 0)
	DBCC CHECKIDENT ([Procedure], reseed, 0)

END