CREATE PROCEDURE [dbo].[CashOrderCreate](
	@UserId NVARCHAR (50),
	@OfficeAddress NVARCHAR (100),
	@Amount DECIMAL (19,4),
	@Currency NVARCHAR (50),
	@IpAddress NVARCHAR (100),
	@Status  NVARCHAR (20)
)
AS  
BEGIN  
    SET NOCOUNT ON;  

    INSERT INTO [dbo].[CashOrders] (UserId, OfficeAddress, Amount, Currency, IpAddress, Status) 
	VALUES(@UserId, @OfficeAddress, @Amount, @Currency, @IpAddress, @Status); 
	SELECT SCOPE_IDENTITY() as Id   
    
END  