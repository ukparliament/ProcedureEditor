CREATE PROCEDURE [dbo].[ImportProcedureBusinessItem-Obsolete]
(
    @TripleStoreId [nvarchar](16),
	@BusinessItemDate [nvarchar](max)=null,
	@WebLink [nvarchar](max)=null,
	@AnsweringBodyTripleStoreId nvarchar(16)=null,
	@ModifiedAt datetimeoffset(0),
	@ModifiedBy [nvarchar](max),
	@NewId int output
)
AS
BEGIN
    SET NOCOUNT ON

	if (@BusinessItemDate is not null)
		set @BusinessItemDate=CONCAT(@BusinessItemDate,'T00:00:00+00:00')

    insert into ProcedureBusinessItem(TripleStoreId, BusinessItemDate, 
		AnsweringBodyId,
		WebLink, ModifiedAt, ModifiedBy)
	select @TripleStoreId, CONVERT(datetimeoffset(0),@BusinessItemDate),
		(select Id from AnsweringBody where TripleStoreId=@AnsweringBodyTripleStoreId),
		@WebLink, @ModifiedAt, @ModifiedBy

	set @NewId=SCOPE_IDENTITY()

END