CREATE PROCEDURE [dbo].[CashOrderGetAll]
AS  
BEGIN  
    SET NOCOUNT ON;  

    SELECT Id, UserId, OfficeAddress, Amount, Currency, IpAddress, Status
    FROM CashOrders
        
END  
