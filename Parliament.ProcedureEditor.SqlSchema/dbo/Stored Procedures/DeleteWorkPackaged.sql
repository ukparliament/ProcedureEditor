CREATE PROCEDURE [dbo].[DeleteWorkPackaged]
(
	@WorkPackagedId int,
	@ModifiedBy [nvarchar](max),
	@IsSuccess bit output
)
AS
BEGIN
    SET NOCOUNT ON

	select top 1 b.Id from ProcedureBusinessItem b
	where b.IsDeleted=0 and b.ProcedureWorkPackagedId=@WorkPackagedId
	union 
	select top 1 p.Id from ProcedureWorkPackagedThingPreceding p
	where p.IsDeleted=0 and 
		((p.ProcedureProposedNegativeStatutoryInstrumentId=@WorkPackagedId) or 
		(p.ProcedureStatutoryInstrumentId=@WorkPackagedId))

	if @@ROWCOUNT>0
		set @IsSuccess=0
	else
		begin
			update ProcedureWorkPackagedThing 
			set 
				IsDeleted=1, 
				ModifiedAt=GETUTCDATE(),
				ModifiedBy=@ModifiedBy
			where Id=@WorkPackagedId
			set @IsSuccess=1
		end

END