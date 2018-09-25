CREATE PROCEDURE [dbo].[InsertUpdateSolrStatutoryInstrument]
(
	@Uri nvarchar(128),
	@SIPrefix nvarchar(32),
	@SINumber int,
	@WebUrl nvarchar(max),
	@ComingIntoForceDate datetimeoffset(0),
	@MadeDate datetimeoffset(0),
	@Title nvarchar(max),
	@SIProcedure nvarchar(50),
	@ComingIntoForceNote nvarchar(max),
	@IsStatutoryInstrument bit,
	@Message nvarchar(50) out
)
AS
BEGIN
    SET NOCOUNT ON
	declare @Id int
	declare @TripleStoreId nvarchar(16)
	declare @IsDeleted bit
	SELECT @Id = Id, @TripleStoreId = TripleStoreId, @IsDeleted = IsDeleted
	FROM [dbo].[SolrStatutoryInstrumentData]
	WHERE [Uri] = @Uri

	IF (@Id is Null)
		BEGIN
			insert into [dbo].[SolrStatutoryInstrumentData] ([Uri],[SIPrefix],[SINumber],[WebUrl],[ComingIntoForceDate],[MadeDate],[Title],[SIProcedure],[ComingIntoForceNote],IsStatutoryInstrument)
			values (@Uri,@SIPrefix,@SINumber,@WebUrl,@ComingIntoForceDate,@MadeDate,@Title,@SIProcedure,@ComingIntoForceNote,@IsStatutoryInstrument)
			select @Message = 'New row added'
		END
	ELSE
		BEGIN
			IF (@TripleStoreId is Null) AND (@IsDeleted = 0)
				BEGIN
					update [dbo].[SolrStatutoryInstrumentData]
						set SIPrefix = @SIPrefix, SINumber = @SINumber, WebUrl = @WebUrl, ComingIntoForceDate = @ComingIntoForceDate, MadeDate=@MadeDate, Title=@Title, SIProcedure = @SIProcedure, ComingIntoForceNote=@ComingIntoForceNote, IsStatutoryInstrument=@IsStatutoryInstrument
					where Id=@Id
					SELECT @Message = 'Current row updated'
				END
			ELSE
				SELECT  @Message = 'No change'
		END
END