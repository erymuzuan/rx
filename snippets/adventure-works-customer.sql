/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [CustomerID]
      ,c.[RxId]
	  ,[PersonID]
      ,[StoreID]
	  ,t.Name as 'Teritorry'
      ,[AccountNumber]
	  ,p.FirstName
	  ,p.LastName
	  ,p.PersonType
  FROM [AdventureWorks].[Sales].[Customer] c
  INNER JOIN [Person].[Person] p
  ON c.PersonID = p.BusinessEntityID
  INNER JOIN Sales.SalesTerritory t
  ON c.TerritoryID = t.TerritoryID
  ORDER BY [LastName], [FirstName]

  /*
  ALTER TABLE Sales.Customer
  ADD [RxId] VARCHAR(50) NULL

  
CREATE PROCEDURE usp_UpdateRxId 
	@AccountNumber varchar(255), 
	@RxId varchar(50)
AS
BEGIN

	SET NOCOUNT ON;
UPDATE [Sales].[Customer]
SET [RXId] = @RxId
WHERE [AccountNumber] = @AccountNumber
END
GO

  */
  SELECT c.[AccountNumber] FROM [Sales].[Customer] c
  INNER JOIN [Person].[Person] p
  ON c.PersonID = p.BusinessEntityID
  WHERE p.[FirstName] = 'Carla' AND p.[LastName] = 'Adams'

  SELECT c.[AccountNumber] FROM [Sales].[Customer] c
  INNER JOIN [Person].[Person] p
  ON c.PersonID = p.BusinessEntityID
  WHERE p.[FirstName] = 'Catherine' AND p.[LastName] = 'Abel'