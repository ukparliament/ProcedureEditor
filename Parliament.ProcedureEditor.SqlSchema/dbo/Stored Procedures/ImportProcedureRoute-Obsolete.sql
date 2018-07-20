CREATE PROCEDURE [dbo].[ImportProcedureRoute-Obsolete]
(
    @TripleStoreId [nvarchar](16),
	@ProcedureTripleStoreId [nvarchar](16),
	@FromProcedureStepTripleStoreId [nvarchar](16),
	@ToProcedureStepTripleStoreId [nvarchar](16),
	@ProcedureRouteTypeName [nvarchar](max),
	@ModifiedAt datetimeoffset(0),
	@ModifiedBy [nvarchar](max)
)
AS
BEGIN
    SET NOCOUNT ON

    insert into ProcedureRoute(TripleStoreId, 
		ProcedureId, 
		FromProcedureStepId,
		ToProcedureStepId, 
		ProcedureRouteTypeId, 
		ModifiedAt, ModifiedBy)
	select @TripleStoreId, 
		(select Id from [Procedure] where TripleStoreId=@ProcedureTripleStoreId),
		(select Id from [ProcedureStep] where TripleStoreId=@FromProcedureStepTripleStoreId),
		(select Id from [ProcedureStep] where TripleStoreId=@ToProcedureStepTripleStoreId),
		(select Id from [ProcedureRouteType] where ProcedureRouteTypeName=@ProcedureRouteTypeName),
		@ModifiedAt, @ModifiedBy

END