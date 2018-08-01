CREATE PROCEDURE [dbo].[DeleteWorkPackageable]
(
	@WorkPackageableId int,
	@ModifiedBy [nvarchar](max),
	@IsSuccess bit output
)
AS
BEGIN
    SET NOCOUNT ON

	select top 1 b.Id from ProcedureBusinessItem b
	where b.IsDeleted=0 and b.ProcedureWorkPackageId=@WorkPackageableId
	union 
	select top 1 p.Id from ProcedureWorkPackageableThingPreceding p
	where p.IsDeleted=0 and 
		((p.PrecedingProcedureWorkPackageableThingId=@WorkPackageableId) or 
		(p.FollowingProcedureWorkPackageableThingId=@WorkPackageableId))

	if @@ROWCOUNT>0
		set @IsSuccess=0
	else
		begin
			update ProcedureWorkPackageableThing 
			set 
				IsDeleted=1, 
				ModifiedAt=GETUTCDATE(),
				ModifiedBy=@ModifiedBy
			where Id=@WorkPackageableId
			set @IsSuccess=1
		end

END