USE [Library]
--USE Test

SELECT 
	game.[Title] Game
	,cc.[Name] CurrentCollection
	,chout.[TimeOut] CheckoutTime
	,chout.[TimeIn] CheckinTime
	,COUNT(player.[Attendee_ID]) NumberOfPlayers
	,rating.Value AvgRating
	,[copy].LibraryID
FROM [dbo].[Games] game
JOIN [Copies] copy on copy.[GameID] = game.[ID]
JOIN CopyCollections cc on cc.ID = copy.CopyCollectionID
JOIN [Checkouts] chout on chout.[Copy_ID] = copy.[ID]
LEFT JOIN [Plays] play on play.[ID] = chout.ID --  = copy.ID
LEFT JOIN Players player on player.[Play_ID] = play.[ID] 
OUTER APPLY (SELECT AVG([Value]) Value
			 FROM [Ratings] rate
			 WHERE rate.[Game_ID] = copy.[GameID] AND rate.[ID] = player.[ID]  
			) AS rating
WHERE [TimeOut] BETWEEN '2019-05-16 08:00:00' AND '2019-05-20 08:00:00' 
GROUP BY 
	game.[Title]
	,[copy].LibraryID
	,cc.[Name]
	,chout.[TimeOut]
	,chout.[TimeIn]
	,rating.Value
