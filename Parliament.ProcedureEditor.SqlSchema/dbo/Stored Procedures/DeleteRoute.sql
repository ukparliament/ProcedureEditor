CREATE PROCEDURE [dbo].[DeleteRoute]
(
	@RouteId int,
	@ModifiedBy [nvarchar](max),
	@IsSuccess bit output
)
AS
BEGIN
    SET NOCOUNT ON

	select top 1 b.Id from ProcedureBusinessItem b
	join ProcedureWorkPackageableThing wp on wp.Id=b.ProcedureWorkPackageId
	join ProcedureBusinessItemProcedureStep bs on bs.ProcedureBusinessItemId=b.Id
	join ProcedureRoute r on ((r.FromProcedureStepId=bs.ProcedureStepId) or (r.ToProcedureStepId=bs.ProcedureStepId)) and r.ProcedureId=wp.ProcedureId
	where b.IsDeleted=0 and r.Id=@RouteId
	
	if @@ROWCOUNT>0
		set @IsSuccess=0
	else
		begin
			update ProcedureRoute 
			set 
				IsDeleted=1, 
				ModifiedAt=GETUTCDATE(),
				ModifiedBy=@ModifiedBy
			where Id=@RouteId
			set @IsSuccess=1
		end

END