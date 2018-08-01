CREATE PROCEDURE [dbo].[DeleteProcedure]
(
	@ProcedureId int,
	@ModifiedBy [nvarchar](max),
	@IsSuccess bit output
)
AS
BEGIN
    SET NOCOUNT ON

	select top 1 Id from ProcedureRoute where IsDeleted=0 and ProcedureId=@ProcedureId
	union
	select top 1 Id from ProcedureWorkPackageableThing where IsDeleted=0 and ProcedureId=@ProcedureId

	if @@ROWCOUNT>0
		set @IsSuccess=0
	else
		begin
			update [Procedure] 
			set 
				IsDeleted=1, 
				ModifiedAt=GETUTCDATE(),
				ModifiedBy=@ModifiedBy
			where Id=@ProcedureId
			set @IsSuccess=1
		end

END