CREATE TABLE [dbo].[CashOrders] (
    [Id]   INT	IDENTITY (1, 1) NOT NULL,
    [UserId] INT, || [UserId] NVARCHAR (50),
    [OfficeAddress] NVARCHAR (100),
    [Amount] DECIMAL (19,4),
    [Currency] NVARCHAR (50),
    [IpAddress] NVARCHAR (100),
    [Status]  NVARCHAR (20)
);