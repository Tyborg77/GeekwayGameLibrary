USE Test;

UPDATE Checkouts SET Attendee_ID = null
UPDATE Copies SET CurrentCheckout_ID = null
UPDATE Checkouts SET Copy_ID = null

DELETE FROM Ratings
DELETE FROM Players
DELETE FROM Plays
DELETE FROM [Checkouts]
DELETE FROM Attendees
DELETE FROM Copies
DELETE FROM Games