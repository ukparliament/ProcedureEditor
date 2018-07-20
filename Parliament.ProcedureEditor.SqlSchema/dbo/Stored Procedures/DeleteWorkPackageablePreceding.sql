create PROCEDURE [dbo].[DeleteWorkPackageablePreceding]
(
	@WorkPackageablePrecedingId int,
	@ModifiedBy [nvarchar](max),
	@IsSuccess bit output
)
AS
BEGIN
    SET NOCOUNT ON

	update ProcedureWorkPackageableThingPreceding 
	set 
		IsDeleted=1, 
		ModifiedAt=GETUTCDATE(),
		ModifiedBy=@ModifiedBy
	where Id=@WorkPackageablePrecedingId
	set @IsSuccess=1
	
END