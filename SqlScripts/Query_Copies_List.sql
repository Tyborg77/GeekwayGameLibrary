USE Library

SELECT Title GameTitle
	  ,COUNT(Copies.LibraryID) Qty
  FROM [dbo].[Games] g
  LEFT JOIN Copies on g.ID = Copies.GameID
  GROUP BY Title