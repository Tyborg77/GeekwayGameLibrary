USE Library

SELECT Title GameTitle
	  ,Copies.LibraryID CopyLibraryID
  FROM [dbo].[Games] g
  LEFT JOIN Copies on g.ID = Copies.GameID
  WHERE Copies.CopyCollectionID = 2