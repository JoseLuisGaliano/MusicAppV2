INSERT INTO [USER]
VALUES ('laulaxula_07', '', '2024-05-21 00:00:00.000', 0, '', '','','');

INSERT INTO [USER]
VALUES ('lauriita_03', '', '2024-05-21 00:00:00.000', 0, 'images/profile_pic.jpg', '','','');

INSERT INTO ARTIST
VALUES ('Taylor Swift', '', '','images/artist_photo.jpg');

INSERT INTO ARTIST
VALUES ('PARTYNEXTDOOR','','', '');

INSERT INTO ARTIST
VALUES ('Nine Inch Nails','','', '');

INSERT INTO ALBUM
VALUES ((SELECT artistID
		 FROM ARTIST
		 WHERE artistName = 'Taylor Swift'), '1989', '2023-01-01 00:00:00.000', 'images/taylor.jpg', (SELECT artistID
																									  FROM ARTIST
																									  WHERE artistName = 'Taylor Swift'), 'POP');

INSERT INTO ALBUM
VALUES ((SELECT artistID
		 FROM ARTIST
		 WHERE artistName = 'PARTYNEXTDOOR'), 'PARTYNEXTDOOR', '2013-01-01 00:00:00.000', '', (SELECT artistID
																							   FROM ARTIST
																							   WHERE artistName = 'PARTYNEXTDOOR'), 'R&B');

INSERT INTO ALBUM
VALUES ((SELECT artistID
		 FROM ARTIST
		 WHERE artistName = 'Nine Inch Nails'), 'With Teeth', '2005-01-01 00:00:00.000', '', (SELECT artistID
																							  FROM ARTIST
																							  WHERE artistName = 'Nine Inch Nails'), 'ROCK');

INSERT INTO SONG
VALUES ('Only', (SELECT artistID
		         FROM ARTIST
				 WHERE artistName = 'Nine Inch Nails'), (SELECT albumID
														 FROM ALBUM
														 WHERE albumTitle = 'With Teeth'), 'ROCK', '00:00:00.000', '2005-01-01 00:00:00.000');

UPDATE ALBUM
SET albumArtist = artistID;

UPDATE SONG
SET albumID = 1
WHERE songID = 1;

UPDATE SONG
SET albumID = 2
WHERE songID = 2;

UPDATE SONG
SET albumID = 3
WHERE songID = 3;

UPDATE SONG
SET albumID = 4
WHERE songID = 4;

UPDATE SONG
SET albumID = 5
WHERE songID = 5;