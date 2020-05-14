CREATE PROCEDURE [dbo].[CashOrderGetById]
    @Id				INT
AS  
BEGIN  
    SET NOCOUNT ON;  
       
   SELECT   Id, UserId, OfficeAddress, Amount, Currency, IpAddress, Status
   FROM  CashOrders CO
   WHERE CO.Id=@Id  
END 