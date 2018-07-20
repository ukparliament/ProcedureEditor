create PROCEDURE [dbo].[ImportProcedureWorkPackageableThingPreceding-Obsolete]
(
    @PrecedingProcedureWorkPackageableThingTripleStoreId [nvarchar](16),
	@FollowingProcedureWorkPackageableThingTripleStoreId [nvarchar](16),
	@ModifiedAt datetimeoffset(0),
	@ModifiedBy [nvarchar](max)
)
AS
BEGIN
    SET NOCOUNT ON

    insert into ProcedureWorkPackageableThingPreceding(PrecedingProcedureWorkPackageableThingId,
		FollowingProcedureWorkPackageableThingId,
		ModifiedAt, ModifiedBy)
	select (select Id from ProcedureWorkPackageableThing where TripleStoreId=@PrecedingProcedureWorkPackageableThingTripleStoreId),
		(select Id from ProcedureWorkPackageableThing where TripleStoreId=@FollowingProcedureWorkPackageableThingTripleStoreId),
		@ModifiedAt, @ModifiedBy

END