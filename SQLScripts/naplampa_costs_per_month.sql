-- FONTOS: itt csak azok vannak listazva, amik postara is lettek mar adva! 
-- ezert erdemes a statisztikakat csak akkor megnezni, ha mar az adott honapban leadott rendelesek 
-- mindegyike postazva is lett

SELECT [PaymentMethod], COUNT(*)
  FROM [SQL2005_436990_naplampa].[dbo].[Order]
  WHERE [OrderStatus] < 65536
  AND [Created] >= '2009-11-01'
  AND [Created] < '2009-12-01'
  AND [CurrencyId] = 17000013 -- csak HUF
  AND [SentOn] IS NOT NULL -- postazva
  AND [OrderStatus] <> 287 -- es nem warranty order
  AND [OrderStatus] <> 2075 -- torott vagy nem mukodik
  GROUP BY [PaymentMethod]
  
  
  
  SELECT
      [ProductId]
      ,SUM([Quantity])
  FROM [SQL2005_436990_naplampa].[dbo].[Order]
  INNER JOIN [OrderProductXRef] ON [Order].[OrderId] = [OrderProductXRef].[OrderId]
  WHERE [OrderStatus] < 65536
  AND [Created] >= '2009-11-01'
  AND [Created] < '2009-12-01'
  AND [CurrencyId] = 17000013
  AND [SentOn] IS NOT NULL
  --AND [OrderStatus] <> 287
 GROUP BY [ProductId]
 
 
 
 
SELECT
      SUM([ProductsCost]) AS ProdCost
      ,SUM([TransactionCost]) AS TransCost
      ,SUM([PackageCost]) AS PackCost
      ,SUM([SendingCost]) AS SendCost
      ,SUM([InsuranceCost]) AS InsCost
      ,SUM([QuantityDiscount]) AS QuantDisc
      ,SUM([CouponDiscount]) AS CouponDisc
      ,SUM([Refund]) AS Refund
      ,SUM([VATTotal]) AS VATTotal
      ,SUM([OrderTotal]) AS Total
  FROM [SQL2005_436990_naplampa].[dbo].[Order]
  WHERE [OrderStatus] < 65536
  AND [Created] >= '2009-11-01'
  AND [Created] < '2009-12-01'
  AND [CurrencyId] = 17000013
  AND [SentOn] IS NOT NULL
  AND [OrderStatus] <> 287

 