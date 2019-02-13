

CREATE PROCEDURE [dbo].[InsertUpdateSolrTreaty]
(
	@Uri nvarchar(128),
	@Prefix nvarchar(32),
	@Number int,
	@WebUrl nvarchar(max),
	@Title nvarchar(max),
	@Series nvarchar(max),
	@IsTreaty bit,
	@Message nvarchar(50) out
)
AS
BEGIN
    SET NOCOUNT ON
	declare @Id int
	declare @TripleStoreId nvarchar(16)
	declare @IsDeleted bit
	SELECT @Id = Id, @TripleStoreId = TripleStoreId, @IsDeleted = IsDeleted
	FROM [dbo].[SolrTreatyData]
	WHERE [Uri] = @Uri

	IF (@Id is Null)
		BEGIN
			insert into [dbo].[SolrTreatyData] ([Uri],[Prefix],[Number],[WebUrl],[Title],[Series],IsTreaty)
			values (@Uri,@Prefix,@Number,@WebUrl,@Title,@Series,@IsTreaty)
			select @Message = 'New row added'
		END
	ELSE
		BEGIN
			IF (@TripleStoreId is Null) AND (@IsDeleted = 0)
				BEGIN
					update [dbo].[SolrTreatyData]
						set Prefix = @Prefix, Number = @Number, WebUrl = @WebUrl, Title=@Title, Series=@Series, IsTreaty=@IsTreaty
					where Id=@Id
					SELECT @Message = 'Current row updated'
				END
			ELSE
				SELECT  @Message = 'No change'
		END
END