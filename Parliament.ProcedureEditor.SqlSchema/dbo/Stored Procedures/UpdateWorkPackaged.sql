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
	@SeriesMembershipTripleStoreId nvarchar(16)=null,
	@SeriesTreatyCitation nvarchar(max)=null,
	@SeriesMembershipTreatyTripleStoreId nvarchar(16)=null,
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

	delete from ProcerdureCountrySeriesMembership where ProcedureTreatyId=@WorkPackagedId
	delete from ProcerdureEuropeanUnionSeriesMembership where ProcedureTreatyId=@WorkPackagedId
	delete from ProcerdureMiscellaneousSeriesMembership where ProcedureTreatyId=@WorkPackagedId
	delete from ProcerdureTreatySeriesMembership where ProcedureTreatyId=@WorkPackagedId
	delete from ProcedureSeriesMembership where TripleStoreId=@SeriesMembershipTripleStoreId
	delete from ProcedureSeriesMembership where TripleStoreId=@SeriesMembershipTreatyTripleStoreId
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

					declare @seriesId int;

					insert into ProcedureSeriesMembership(TripleStoreId, Citation)
					values (@SeriesMembershipTripleStoreId, @SeriesCitation)

					set @seriesId=SCOPE_IDENTITY()
					
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

					if (@IsTreatySeriesMembership=1)
					begin
						declare @treatySeriesId int=null
						insert into ProcedureSeriesMembership(TripleStoreId, Citation)
						values (@SeriesMembershipTreatyTripleStoreId, @SeriesTreatyCitation)

						set @treatySeriesId=SCOPE_IDENTITY()
						insert into ProcerdureTreatySeriesMembership (Id, ProcedureTreatyId)
						values (@treatySeriesId, @WorkPackagedId)						
					end					
				end

END