﻿CREATE PROCEDURE [dbo].[DeleteStep]
(
	@StepId int,
	@ModifiedBy [nvarchar](max),
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
			delete from ProcedureStep where Id=@StepId
			delete from ProcedureStepHouse where ProcedureStepId=@StepId
			set @IsSuccess=1
		end

END