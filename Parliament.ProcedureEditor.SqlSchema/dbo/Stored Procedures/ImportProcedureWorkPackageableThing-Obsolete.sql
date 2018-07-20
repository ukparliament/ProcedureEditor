CREATE PROCEDURE [dbo].[ImportProcedureWorkPackageableThing-Obsolete]
(
    @TripleStoreId [nvarchar](16),
	@ProcedureWorkPackageableThingName [nvarchar](max),
	@StatutoryInstrumentNumber [nvarchar](32)=null,
	@StatutoryInstrumentNumberPrefix [nvarchar](32)=null,
	@StatutoryInstrumentNumberYear [int]=null,
	@ComingIntoForceDate [nvarchar](max)=null,
	@ComingIntoForceNote [nvarchar](max)=null,
	@TimeLimitForObjectionEndDate [nvarchar](max)=null,
	@WebLink [nvarchar](max)=null,
	@ProcedureWorkPackageableThingTypeName nvarchar(max)=null,
	@ProcedureTripleStoreId [nvarchar](16),
	@ProcedureWorkPackageTripleStoreId [nvarchar](16),
	@ModifiedAt datetimeoffset(0),
	@ModifiedBy [nvarchar](max)
)
AS
BEGIN
    SET NOCOUNT ON

	if (@ComingIntoForceDate is not null)
		set @ComingIntoForceDate=CONCAT(@ComingIntoForceDate,'T00:00:00+00:00')

	if (@TimeLimitForObjectionEndDate is not null)
		set @TimeLimitForObjectionEndDate=CONCAT(@TimeLimitForObjectionEndDate,'T00:00:00+00:00')

    insert into ProcedureWorkPackageableThing(TripleStoreId, 
		ProcedureWorkPackageableThingName, StatutoryInstrumentNumber,
		StatutoryInstrumentNumberPrefix, StatutoryInstrumentNumberYear,
		ComingIntoForceDate, ComingIntoForceNote, 
		TimeLimitForObjectionEndDate,
		WebLink, 
		ProcedureWorkPackageableThingTypeId,
		ProcedureWorkPackageTripleStoreId,
		ProcedureId,
		ModifiedAt, ModifiedBy)
	select @TripleStoreId, 
		@ProcedureWorkPackageableThingName, @StatutoryInstrumentNumber,
		@StatutoryInstrumentNumberPrefix, @StatutoryInstrumentNumberYear,
		CONVERT(datetimeoffset(0),@ComingIntoForceDate), @ComingIntoForceNote, 
		CONVERT(datetimeoffset(0),@TimeLimitForObjectionEndDate),
		@WebLink, 
		(select Id from ProcedureWorkPackageableThingType where ProcedureWorkPackageableThingTypeName=@ProcedureWorkPackageableThingTypeName),
		@ProcedureWorkPackageTripleStoreId,
		(select Id from [Procedure] where TripleStoreId=@ProcedureTripleStoreId),
		@ModifiedAt, @ModifiedBy

END