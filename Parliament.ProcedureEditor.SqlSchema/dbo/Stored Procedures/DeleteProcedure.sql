CREATE PROCEDURE [dbo].[DeleteProcedure]
(
	@ProcedureId int,
	@IsSuccess bit output
)
AS
BEGIN
    SET NOCOUNT ON

	select top 1 Id from ProcedureRouteProcedure where ProcedureId=@ProcedureId
	union
	select top 1 Id from ProcedureWorkPackagedThing where ProcedureId=@ProcedureId

	if @@ROWCOUNT>0
		set @IsSuccess=0
	else
		begin
			delete from [Procedure] where Id=@ProcedureId
			set @IsSuccess=1
		end

END