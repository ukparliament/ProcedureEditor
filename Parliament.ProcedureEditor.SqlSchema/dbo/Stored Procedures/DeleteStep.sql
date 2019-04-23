CREATE PROCEDURE [dbo].[DeleteStep]
(
	@StepId int,
	@IsSuccess bit output
)
AS
BEGIN
    SET NOCOUNT ON

	select top 1 Id from ProcedureRoute where ((FromProcedureStepId=@StepId) or (ToProcedureStepId=@StepId))
	union
	select top 1 Id from ProcedureBusinessItemProcedureStep where ProcedureStepId=@StepId

	if @@ROWCOUNT>0
		set @IsSuccess=0
	else
		begin
			delete from ProcedureStepHouse where ProcedureStepId=@StepId

			delete from ProcedureStepAlongsideStep where ProcedureStepId=@StepId

			declare @PublicationId int;

			select @PublicationId=psp.Id from ProcedureStepPublication psp
			join ProcedureStep ps on ps.ProcedureStepPublicationId=psp.Id
			where ps.Id=@StepId

			delete from ProcedureStep where Id=@StepId	
			
			delete from ProcedureStepPublication  where Id=@PublicationId
	
			set @IsSuccess=1
		end

END