CREATE PROCEDURE [dbo].[UpdateWorkPackaged]
(
	@WorkPackagedId int,
	@WebLink nvarchar(max),
	@ProcedureId int,
	@WorkPackagedKind int,
	@WorkPackagedThingName nvarchar(max),
	@StatutoryInstrumentNumber int=null,
	@StatutoryInstrumentNumberPrefix nvarchar(32)=null,
	@StatutoryInstrumentNumberYear int=null,
	@ComingIntoForceNote nvarchar(max)=null,
	@ComingIntoForceDate datetimeoffset(0)=null,
	@MadeDate datetimeoffset(0)=null,
	@LeadGovernmentOrganisationTripleStoreId nvarchar(16)=null,
	@SeriesCitation nvarchar(max)=null,
	@SeriesMembershipTripleStoreId nvarchar(16)=null,	/*always new*/
	@SeriesTreatyCitation nvarchar(max)=null,
	@SeriesMembershipTreatyTripleStoreId nvarchar(16)=null,	/*always new*/
	@IsCountrySeriesMembership bit=null,
	@IsEuropeanUnionSeriesMembership bit=null,
	@IsMiscellaneousSeriesMembership bit=null,
	@IsTreatySeriesMembership bit=null,
	@ModifiedBy [nvarchar](max)
)
AS
BEGIN
    SET NOCOUNT ON
	
	delete from ProcedureStatutoryInstrument where Id=@WorkPackagedId
	delete from ProcedureProposedNegativeStatutoryInstrument where Id=@WorkPackagedId

	declare @seriesId int
	select @seriesId=s.Id from ProcedureSeriesMembership s
	left join ProcerdureCountrySeriesMembership c on c.Id=s.Id
	left join ProcerdureEuropeanUnionSeriesMembership e on e.Id=s.Id
	left join ProcerdureMiscellaneousSeriesMembership m on m.Id=s.Id					
	where coalesce(c.ProcedureTreatyId,e.ProcedureTreatyId,m.ProcedureTreatyId)=@WorkPackagedId
						
	delete from ProcerdureCountrySeriesMembership where ProcedureTreatyId=@WorkPackagedId
	delete from ProcerdureEuropeanUnionSeriesMembership where ProcedureTreatyId=@WorkPackagedId
	delete from ProcerdureMiscellaneousSeriesMembership where ProcedureTreatyId=@WorkPackagedId
	delete from ProcedureTreaty where Id=@WorkPackagedId
	
	update ProcedureWorkPackagedThing 
		set WebLink=@WebLink,
			ProcedureId=@ProcedureId,
			ModifiedAt=GETUTCDATE(),
			ModifiedBy=@ModifiedBy
	where Id=@WorkPackagedId
	
	if (@WorkPackagedKind=1)
	begin
		insert into ProcedureStatutoryInstrument (Id, ProcedureStatutoryInstrumentName, StatutoryInstrumentNumber,
			StatutoryInstrumentNumberPrefix, StatutoryInstrumentNumberYear, ComingIntoForceNote,
			ComingIntoForceDate, MadeDate)
		values (@WorkPackagedId, @WorkPackagedThingName, @StatutoryInstrumentNumber,
			@StatutoryInstrumentNumberPrefix, @StatutoryInstrumentNumberYear, @ComingIntoForceNote,
			@ComingIntoForceDate, @MadeDate)
	end
	else
		if (@WorkPackagedKind=2)
		begin
			insert into ProcedureProposedNegativeStatutoryInstrument (Id, ProcedureProposedNegativeStatutoryInstrumentName)
			values (@WorkPackagedId, @WorkPackagedThingName)
		end
			else
				if (@WorkPackagedKind=3)
				begin
					
					insert into ProcedureTreaty (Id, ProcedureTreatyName,
						TreatyNumber, TreatyPrefix, ComingIntoForceNote,
						ComingIntoForceDate, LeadGovernmentOrganisationTripleStoreId)
					values (@WorkPackagedId, @WorkPackagedThingName,
						@StatutoryInstrumentNumber, @StatutoryInstrumentNumberPrefix, @ComingIntoForceNote,
						@ComingIntoForceDate, @LeadGovernmentOrganisationTripleStoreId)

					if (@seriesId is null)
					begin
						insert into ProcedureSeriesMembership(TripleStoreId, Citation)
						values (@SeriesMembershipTripleStoreId, @SeriesCitation)

						set @seriesId=SCOPE_IDENTITY()
					end
					else
					begin
						update ProcedureSeriesMembership
							set Citation=@SeriesCitation
						where Id=@seriesId
					end

					if (@IsCountrySeriesMembership=1)
					begin
						insert into ProcerdureCountrySeriesMembership (Id, ProcedureTreatyId)
						values (@seriesId, @WorkPackagedId)
					end
					if (@IsEuropeanUnionSeriesMembership=1)
					begin
						insert into ProcerdureEuropeanUnionSeriesMembership (Id, ProcedureTreatyId)
						values (@seriesId, @WorkPackagedId)
					end
					if (@IsMiscellaneousSeriesMembership=1)
					begin
						insert into ProcerdureMiscellaneousSeriesMembership (Id, ProcedureTreatyId)
						values (@seriesId, @WorkPackagedId)
					end
					declare @treatySeriesId int=null

					select @treatySeriesId=s.Id from ProcedureSeriesMembership s
					join ProcerdureTreatySeriesMembership t on t.Id=s.Id
					where t.ProcedureTreatyId=@WorkPackagedId
			
					if (@IsTreatySeriesMembership=1)
					begin
						if (@treatySeriesId is null)
						begin
							insert into ProcedureSeriesMembership(TripleStoreId, Citation)
							values (@SeriesMembershipTreatyTripleStoreId, @SeriesTreatyCitation)

							set @treatySeriesId=SCOPE_IDENTITY()
							insert into ProcerdureTreatySeriesMembership (Id, ProcedureTreatyId)
							values (@treatySeriesId, @WorkPackagedId)
						end
						else
						begin
							update ProcedureSeriesMembership
								set Citation=@SeriesTreatyCitation
							where Id=@treatySeriesId
						end
					end
					else
					begin
						delete from ProcerdureTreatySeriesMembership where ProcedureTreatyId=@WorkPackagedId
						delete from ProcedureSeriesMembership where Id=@treatySeriesId
					end
				end

END