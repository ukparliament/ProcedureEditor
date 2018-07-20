CREATE PROCEDURE [dbo].[ImportProcedureBusinessItemProcedureWorkPackage-Obsolete]
(
    @BusinessItemId int,
	@ProcedureWorkPackageTripleStoreId nvarchar(16)	
)
AS
BEGIN
    SET NOCOUNT ON

    insert into ProcedureBusinessItemProcedureWorkPackage(ProcedureBusinessItemId, 
		ProcedureWorkPackageId)
	select @BusinessItemId,
		(select Id from ProcedureWorkPackageableThing where ProcedureWorkPackageTripleStoreId=@ProcedureWorkPackageTripleStoreId)

END