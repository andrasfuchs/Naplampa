/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [DiscountId]
      ,[Code]
      ,[MinimumPartnerRank]
      ,[MinimumQuantity]
      ,[Multiplier]
      ,[ValidFrom]
      ,[ValidUntil]
      ,[ProductId]
      ,[CountryId]
  FROM [SQL2005_436990_naplampa].[dbo].[Discount]
  WHERE (([ValidUntil] IS NULL)
  OR ([ValidUntil] > GETDATE()))
  AND ([ProductId] IS NOT NULL)
  ORDER BY [ProductId], [CountryId], [ValidFrom]