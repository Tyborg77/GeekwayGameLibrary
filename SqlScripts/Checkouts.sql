--USE [Library]
--USE PlayAndWin
USE Test

SELECT 
	game.[Title] Game
	,chout.[TimeOut] CheckoutTime
	,chout.[TimeIn] CheckinTime
	--,COUNT(player.[Attendee_ID]) NumberOfPlayers
	--,rating.Value AvgRating --ISNULL(rating.Value,0) May want to handle in case there are no returns from the OUTER APPLY, such as a specific game got no ratings because no rated it for their play
FROM [dbo].[Games] game --[PlayAndWin].[dbo].Games game
JOIN [Copies] copy on copy.[GameID] = game.[ID]
JOIN [Checkouts] chout on chout.[Copy_ID] = copy.[ID]
JOIN [Plays] play on play.[ID] = chout.ID --  = copy.ID
--JOIN Players player on player.[Play_ID] = play.[ID] 
--LEFT JOIN Ratings rating on rating.Game_ID = copy.GameID
--OUTER APPLY (SELECT AVG([Value]) Value
--			 FROM [Ratings] rate
--			 WHERE rate.[Game_ID] = copy.[GameID] AND rate.[ID] = player.[ID]  --I see this a foreign key relationship, but it doesn't make any sense.
--																			   --I think a field needs to be added to the Ratings table to hold the Players ID or the Attendee_ID
--			) AS rating

WHERE [TimeOut] BETWEEN '2019-01-17 18:00:00' AND '2019-01-21 08:00:00'

--[TimeOut] >= '2019-01-17 18:00:00' AND [TimeOut] <= '2019-01-21 08:00:00'
GROUP BY 
	game.[Title]
	,chout.[TimeOut]
	,chout.[TimeIn]
	--,rating.Value