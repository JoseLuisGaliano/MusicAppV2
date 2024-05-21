--USER
INSERT INTO [USER] (username, email, dateJoined, isActive, profilePicture, subscriptionPlan, hashedPassword, salt)
VALUES ('nico', 'nico@gmail.com', '2024-05-20T00:00:00', 1, 'nico.jpg', 'Gold', '', '');

INSERT INTO [USER] (username, email, dateJoined, isActive, profilePicture, subscriptionPlan, hashedPassword, salt)
VALUES ('pablo', 'pablo@gmail.com', '2024-05-20T00:00:00', 1, 'pablo.jpg', 'Silver', '', '');


--ARTIST
INSERT INTO ARTIST (artistName, biography, genre, artistPicture)
VALUES ('Taylor Swift', 'Taylor Alison Swift is an American singer-songwriter.', 'Pop', 'taylor_swift.jpg');

INSERT INTO ARTIST (artistName, biography, genre, artistPicture)
VALUES ('Ed Sheeran', 'Edward Christopher Sheeran is an English singer-songwriter.', 'Pop', 'ed_sheeran.jpg');

INSERT INTO ARTIST (artistName, biography, genre, artistPicture)
VALUES ('Drake', 'Aubrey Drake Graham is a Canadian rapper, singer, and actor.', 'Hip-Hop', 'drake.jpg');

INSERT INTO ARTIST (artistName, biography, genre, artistPicture)
VALUES ('Adele', 'Adele Laurie Blue Adkins is an English singer-songwriter.', 'Pop', 'adele.jpg');

INSERT INTO ARTIST (artistName, biography, genre, artistPicture)
VALUES ('Beyoncé', 'Beyoncé Giselle Knowles-Carter is an American singer, songwriter, and actress.', 'R&B', 'beyonce.jpg');



--SONG
INSERT INTO SONG (songTitle, artistID, albumID, songGenre, songDuration, songReleaseDate)
VALUES ('Shake It Off', 1, NULL, 'Pop', '00:03:39', '2014-08-18T00:00:00');

INSERT INTO SONG (songTitle, artistID, albumID, songGenre, songDuration, songReleaseDate)
VALUES ('Shape of You', 2, NULL, 'Pop', '00:03:53', '2017-01-06T00:00:00');

INSERT INTO SONG (songTitle, artistID, albumID, songGenre, songDuration, songReleaseDate)
VALUES ('Hotline Bling', 3, NULL, 'Hip-Hop', '00:04:27', '2015-07-31T00:00:00');

INSERT INTO SONG (songTitle, artistID, albumID, songGenre, songDuration, songReleaseDate)
VALUES ('Hello', 4, NULL, 'Pop', '00:04:55', '2015-10-23T00:00:00');

INSERT INTO SONG (songTitle, artistID, albumID, songGenre, songDuration, songReleaseDate)
VALUES ('Single Ladies (Put a Ring on It)', 5, NULL, 'R&B', '00:03:13', '2008-10-13T00:00:00');


-- ALBUM
INSERT INTO ALBUM (artistID, albumTitle, albumReleaseDate, albumPicture, albumGenre)
VALUES (1, '1989', '2014-10-27T00:00:00', '1989_album.jpg', 'Pop');

INSERT INTO ALBUM (artistID, albumTitle, albumReleaseDate, albumPicture, albumGenre)
VALUES (2, '÷ (Divide)', '2017-03-03T00:00:00', 'divide_album.jpg', 'Pop');

INSERT INTO ALBUM (artistID, albumTitle, albumReleaseDate, albumPicture, albumGenre)
VALUES (3, 'Views', '2016-04-29T00:00:00', 'views_album.jpg', 'Hip-Hop');

INSERT INTO ALBUM (artistID, albumTitle, albumReleaseDate, albumPicture, albumGenre)
VALUES (4, '25', '2015-11-20T00:00:00', '25_album.jpg', 'Pop');

INSERT INTO ALBUM (artistID, albumTitle, albumReleaseDate, albumPicture, albumGenre)
VALUES (5, 'I Am... Sasha Fierce', '2008-11-12T00:00:00', 'sasha_fierce_album.jpg', 'R&B');



--PLAYLIST
INSERT INTO PLAYLIST (ownerID, playlistName, playlistDescription, playlistCreationDate)
VALUES (1, 'Favorites', 'My favorite songs.', '2024-05-20T00:00:00');

INSERT INTO PLAYLIST (ownerID, playlistName, playlistDescription, playlistCreationDate)
VALUES (2, 'Workout Mix', 'Great tunes to keep me motivated.', '2024-05-21T00:00:00');

INSERT INTO PLAYLIST (ownerID, playlistName, playlistDescription, playlistCreationDate)
VALUES (1, 'Chill Vibes', 'Relaxing music for a peaceful evening.', '2024-05-22T00:00:00');

INSERT INTO PLAYLIST (ownerID, playlistName, playlistDescription, playlistCreationDate)
VALUES (2, 'Road Trip', 'Awesome tracks for a long drive.', '2024-05-23T00:00:00');

INSERT INTO PLAYLIST (ownerID, playlistName, playlistDescription, playlistCreationDate)
VALUES (2, 'Throwback Jams', 'Classic hits from the past.', '2024-05-24T00:00:00');


--PLAYLISTSONG
INSERT INTO PLAYLISTSONG (playlistID, songID)
VALUES (10, 1);  -- Añade la canción con ID 1 a la playlist con ID 1

INSERT INTO PLAYLISTSONG (playlistID, songID)
VALUES (6, 1);  

INSERT INTO PLAYLISTSONG (playlistID, songID)
VALUES (7, 4);  

INSERT INTO PLAYLISTSONG (playlistID, songID)
VALUES (8, 5);  

INSERT INTO PLAYLISTSONG (playlistID, songID)
VALUES (9, 3);  

INSERT INTO PLAYLISTSONG (playlistID, songID)
VALUES (10, 2);  

INSERT INTO PLAYLISTSONG (playlistID, songID)
VALUES (8, 3);  

INSERT INTO PLAYLISTSONG (playlistID, songID)
VALUES (6, 4);  


--FEEDBACK

INSERT INTO FEEDBACK (userID, songID, rating, Comment, feedbackDate)
VALUES (1, 1, 5, 'Great song! Love the lyrics.', '2024-05-20T00:00:00');


INSERT INTO FEEDBACK (userID, songID, rating, Comment, feedbackDate)
VALUES (2, 2, 4, 'Nice beat and melody.', '2024-05-21T00:00:00');


INSERT INTO FEEDBACK (userID, songID, rating, Comment, feedbackDate)
VALUES (2, 3, 3, 'It is okay, not my favorite.', '2024-05-22T00:00:00');


INSERT INTO FEEDBACK (userID, songID, rating, Comment, feedbackDate)
VALUES (1, 4, 5, 'Absolutely love it!', '2024-05-23T00:00:00');


--EVENTS
INSERT INTO EVENT (artistID, eventName, eventDescription, eventLocation, eventDate, eventTicketPrice)
VALUES (1, 'Concert at Central Park', 'An amazing live concert in Central Park.', 'Central Park, New York', '2024-06-15T19:00:00', 75.00);


INSERT INTO EVENT (artistID, eventName, eventDescription, eventLocation, eventDate, eventTicketPrice)
VALUES (2, 'Album Release Party', 'Join us for the release of the new album.', 'Madison Square Garden, New York', '2024-07-10T20:00:00', 50.00);


INSERT INTO EVENT (artistID, eventName, eventDescription, eventLocation, eventDate, eventTicketPrice)
VALUES (3, 'Summer Festival', 'A festival featuring multiple artists.', 'Grant Park, Chicago', '2024-08-05T14:00:00', 100.00);

INSERT INTO EVENT (artistID, eventName, eventDescription, eventLocation, eventDate, eventTicketPrice)
VALUES (4, 'Charity Gala', 'A special performance at a charity gala.', 'The Beverly Hills Hotel, Los Angeles', '2024-09-25T18:30:00', 150.00);


INSERT INTO EVENT (artistID, eventName, eventDescription, eventLocation, eventDate, eventTicketPrice)
VALUES (5, 'Private Concert', 'An exclusive private concert.', 'The O2 Arena, London', '2024-10-10T21:00:00', 200.00);


--TICKET
INSERT INTO TICKET (eventID, userID, purchaseDate, ticketPrice, ticketType)
VALUES (1, 1, '2024-05-20T10:00:00', 75.00, 'VIP');

-- Ticket 2
INSERT INTO TICKET (eventID, userID, purchaseDate, ticketPrice, ticketType)
VALUES (2, 2, '2024-05-21T11:30:00', 50.00, 'General Admission');

-- Ticket 3
INSERT INTO TICKET (eventID, userID, purchaseDate, ticketPrice, ticketType)
VALUES (3, 1, '2024-05-22T14:45:00', 100.00, 'Early Bird');

-- Ticket 4
INSERT INTO TICKET (eventID, userID, purchaseDate, ticketPrice, ticketType)
VALUES (4, 2, '2024-05-23T09:15:00', 150.00, 'Premium');

-- Ticket 5
INSERT INTO TICKET (eventID, userID, purchaseDate, ticketPrice, ticketType)
VALUES (5, 2, '2024-05-24T16:00:00', 200.00, 'Exclusive');
