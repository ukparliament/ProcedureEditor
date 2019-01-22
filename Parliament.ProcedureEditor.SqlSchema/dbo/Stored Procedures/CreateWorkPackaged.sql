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
					insert into ProcedureTreaty (Id, ProcedureTreatyName)
					values (@id, @WorkPackagedThingName)
				end

	if (@SolarFeedId is not null)
	begin
		update SolrStatutoryInstrumentData
                set TripleStoreId=@TripleStoreId 
		where Id=@SolarFeedId
	end

END