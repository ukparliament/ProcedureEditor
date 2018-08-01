CREATE PROCEDURE [dbo].[DeleteStep]
(
	@StepId int,
	@ModifiedBy [nvarchar](max),
	@IsSuccess bit output
)
AS
BEGIN
    SET NOCOUNT ON

	select top 1 Id from ProcedureRoute where IsDeleted=0 and ((FromProcedureStepId=@StepId) or (ToProcedureStepId=@StepId))
	union
	select top 1 Id from ProcedureBusinessItemProcedureStep where ProcedureStepId=@StepId

	if @@ROWCOUNT>0
		set @IsSuccess=0
	else
		begin
			update ProcedureStep
			set 
				IsDeleted=1, 
				ModifiedAt=GETUTCDATE(),
				ModifiedBy=@ModifiedBy
			where Id=@StepId
			set @IsSuccess=1

			delete from ProcedureStepHouse where ProcedureStepId=@StepId
		end

END