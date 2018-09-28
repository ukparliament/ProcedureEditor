CREATE PROCEDURE [dbo].[DeleteBusinessItem]
(
	@BusinessItemId int,
	@IsSuccess bit output
)
AS
BEGIN
    SET NOCOUNT ON

	select top 1 Id from ProcedureLaying where ProcedureBusinessItemId=@BusinessItemId
	
	if @@ROWCOUNT>0
		set @IsSuccess=0
	else
		begin
			delete from ProcedureBusinessItemProcedureStep where ProcedureBusinessItemId=@BusinessItemId
			delete from ProcedureBusinessItem where Id=@BusinessItemId
			set @IsSuccess=1
		end

END