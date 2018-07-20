create PROCEDURE [dbo].[DeleteBusinessItem]
(
	@BusinessItemId int,
	@ModifiedBy [nvarchar](max),
	@IsSuccess bit output
)
AS
BEGIN
    SET NOCOUNT ON

	update ProcedureBusinessItem
	set 
		IsDeleted=1, 
		ModifiedAt=GETUTCDATE(),
		ModifiedBy=@ModifiedBy
	where Id=@BusinessItemId
	set @IsSuccess=1
	
END