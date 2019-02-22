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
			
			declare @seriesId int
			select top 1 @seriesId=s.Id from ProcedureSeriesMembership s
			left join ProcerdureCountrySeriesMembership c on c.Id=s.Id
			left join ProcerdureEuropeanUnionSeriesMembership e on e.Id=s.Id
			left join ProcerdureMiscellaneousSeriesMembership m on m.Id=s.Id
			left join ProcerdureTreatySeriesMembership t on t.Id=s.Id
			where coalesce(c.ProcedureTreatyId,e.ProcedureTreatyId,m.ProcedureTreatyId,t.ProcedureTreatyId)=@WorkPackagedId
			
			delete from ProcerdureCountrySeriesMembership where ProcedureTreatyId=@WorkPackagedId
			delete from ProcerdureEuropeanUnionSeriesMembership where ProcedureTreatyId=@WorkPackagedId
			delete from ProcerdureMiscellaneousSeriesMembership where ProcedureTreatyId=@WorkPackagedId
			delete from ProcerdureTreatySeriesMembership where ProcedureTreatyId=@WorkPackagedId
			delete from ProcedureSeriesMembership where Id=@seriesId
			
			delete from ProcedureTreaty where Id=@WorkPackagedId
			delete from ProcedureWorkPackagedThing where Id=@WorkPackagedId
			set @IsSuccess=1
		end

END