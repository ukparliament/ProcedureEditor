create PROCEDURE [dbo].[ImportProcedureBusinessItemProcedureStep-Obsolete]
(
    @BusinessItemId int,
	@ProcedureStepTripleStoreId nvarchar(16)	
)
AS
BEGIN
    SET NOCOUNT ON

    insert into ProcedureBusinessItemProcedureStep(ProcedureBusinessItemId, 
		ProcedureStepId)
	select @BusinessItemId,
		(select Id from ProcedureStep where TripleStoreId=@ProcedureStepTripleStoreId)

END