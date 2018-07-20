create PROCEDURE [dbo].[ImportProcedureStepHouse-Obsolete]
(
	@ProcedureStepId int,
    @HouseTripleStoreId [nvarchar](16)
)
AS
BEGIN
    SET NOCOUNT ON

	insert into ProcedureStepHouse(ProcedureStepId,
		HouseId)
	select @ProcedureStepId,
		(select Id from House where TripleStoreId=@HouseTripleStoreId)
	
END