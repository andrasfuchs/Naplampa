/****** Script for SelectTopNRows command from SSMS  ******/
SELECT [Product].[ProductId], [Product].[Name], SUM([Quantity])
  FROM [SQL2005_436990_naplampa].[dbo].[OrderProductXRef]
INNER JOIN [Order] ON [OrderProductXRef].[OrderId] = [Order].[OrderId]  
INNER JOIN [Product] ON [OrderProductXRef].[ProductId] = [Product].[ProductId]  
WHERE ([OrderStatus] < 65536) AND ([Quantity] < 40)
GROUP BY [Product].[ProductId], [Product].[Name]
  
  
SELECT [Product].[ProductId], [Product].[Name], SUM(Quantity)
FROM  [SQL2005_436990_naplampa].[dbo].[StockItem]
INNER JOIN [Product] ON [StockItem].[ProductId] = [Product].[ProductId]  
WHERE [OwnerPartnerId] = 21000000
GROUP BY [Product].[ProductId], [Product].[Name]