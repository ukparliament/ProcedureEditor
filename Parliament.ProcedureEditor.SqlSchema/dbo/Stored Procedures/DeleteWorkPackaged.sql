CREATE PROCEDURE [dbo].[DeleteWorkPackaged]
(
	@WorkPackagedId int,
	@IsSuccess bit output
)
AS
BEGIN
    SET NOCOUNT ON

	select top 1 b.Id from ProcedureBusinessItem b
	where b.ProcedureWorkPackageId=@WorkPackagedId
	union 
	select top 1 p.Id from ProcedureWorkPackagedThingPreceding p
	where ((p.WorkPackagedIsFollowedById=@WorkPackagedId) or 
		(p.WorkPackagedIsPrecededById=@WorkPackagedId))

	if @@ROWCOUNT>0
		set @IsSuccess=0
	else
		begin
			delete from ProcedureStatutoryInstrument where Id=@WorkPackagedId
			delete from ProcedureProposedNegativeStatutoryInstrument where Id=@WorkPackagedId
			delete from ProcedureTreaty where Id=@WorkPackagedId
			delete from ProcedureWorkPackagedThing where Id=@WorkPackagedId
			set @IsSuccess=1
		end

END