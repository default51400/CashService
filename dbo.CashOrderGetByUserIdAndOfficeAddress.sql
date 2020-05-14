CREATE PROCEDURE [dbo].[CashOrderGetByClientIdAndDepartemntAddress]
    @UserId		nvarchar(50),
	@OfficeAddress nvarchar(100)
AS  
BEGIN  
    SET NOCOUNT ON;  
       
   SELECT   Id, UserId, OfficeAddress, Amount, Currency, IpAddress, Status
   FROM  CashOrders CO
   WHERE CO.UserId=@UserId AND CO.OfficeAddress=@OfficeAddress  
END 