CREATE PROCEDURE [dbo].[CreateWorkPackaged]
(
	@TripleStoreId nvarchar(16),
	@WebLink nvarchar(max),
	@ProcedureWorkPackageTripleStoreId nvarchar(16),
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
	@SeriesTreatyCitation nvarchar(max)=null,
	@SeriesMembershipTripleStoreId nvarchar(16)=null,
	@SeriesMembershipTreatyTripleStoreId nvarchar(16)=null,
	@IsCountrySeriesMembership bit=null,
	@IsEuropeanUnionSeriesMembership bit=null,
	@IsMiscellaneousSeriesMembership bit=null,
	@IsTreatySeriesMembership bit=null,
	@SolarFeedId int=null,
	@ModifiedBy [nvarchar](max)
)
AS
BEGIN
    SET NOCOUNT ON
	declare @id int

	insert into ProcedureWorkPackagedThing (TripleStoreId, WebLink, ProcedureWorkPackageTripleStoreId,
		ProcedureId, ModifiedAt, ModifiedBy)
	values (@TripleStoreId, @WebLink, @ProcedureWorkPackageTripleStoreId,
		@ProcedureId, GETUTCDATE(), @ModifiedBy)
	
	set @id=SCOPE_IDENTITY()
	
	if (@WorkPackagedKind=1)
	begin
		insert into ProcedureStatutoryInstrument (Id, ProcedureStatutoryInstrumentName, StatutoryInstrumentNumber,
			StatutoryInstrumentNumberPrefix, StatutoryInstrumentNumberYear, ComingIntoForceNote,
			ComingIntoForceDate, MadeDate)
		values (@id, @WorkPackagedThingName, @StatutoryInstrumentNumber,
			@StatutoryInstrumentNumberPrefix, @StatutoryInstrumentNumberYear, @ComingIntoForceNote,
			@ComingIntoForceDate, @MadeDate)
	end
	else
		if (@WorkPackagedKind=2)
		begin
			insert into ProcedureProposedNegativeStatutoryInstrument (Id, ProcedureProposedNegativeStatutoryInstrumentName)
			values (@id, @WorkPackagedThingName)
		end
			else
				if (@WorkPackagedKind=3)
				begin
					declare @seriesId int

					insert into ProcedureTreaty (Id, ProcedureTreatyName, TreatyNumber, TreatyPrefix,
						ComingIntoForceNote, ComingIntoForceDate, LeadGovernmentOrganisationTripleStoreId)
					values (@id, @WorkPackagedThingName, @StatutoryInstrumentNumber, @StatutoryInstrumentNumberPrefix,
						@ComingIntoForceNote, @ComingIntoForceDate, @LeadGovernmentOrganisationTripleStoreId)

					insert into ProcedureSeriesMembership(TripleStoreId, Citation)
					values (@SeriesMembershipTripleStoreId, @SeriesCitation)

					set @seriesId=SCOPE_IDENTITY()

					if (@IsCountrySeriesMembership=1)
					begin
						insert into ProcerdureCountrySeriesMembership (Id, ProcedureTreatyId)
						values (@seriesId, @id)
					end
					if (@IsEuropeanUnionSeriesMembership=1)
					begin
						insert into ProcerdureEuropeanUnionSeriesMembership (Id, ProcedureTreatyId)
						values (@seriesId, @id)
					end
					if (@IsMiscellaneousSeriesMembership=1)
					begin
						insert into ProcerdureMiscellaneousSeriesMembership (Id, ProcedureTreatyId)
						values (@seriesId, @id)
					end
					if (@IsTreatySeriesMembership=1)
					begin
						insert into ProcedureSeriesMembership(TripleStoreId, Citation)
						values (@SeriesMembershipTreatyTripleStoreId, @SeriesTreatyCitation)

						set @seriesId=SCOPE_IDENTITY()
						insert into ProcerdureTreatySeriesMembership (Id, ProcedureTreatyId)
						values (@seriesId, @id)
					end
				end

	if (@SolarFeedId is not null)
	begin
		if (@WorkPackagedKind=3)
		begin
			update SolrTreatyData
					set TripleStoreId=@TripleStoreId 
			where Id=@SolarFeedId
		end
		else
			begin
				update SolrStatutoryInstrumentData
						set TripleStoreId=@TripleStoreId 
				where Id=@SolarFeedId
			end
	end

END