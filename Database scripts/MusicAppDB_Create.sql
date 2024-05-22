
-- USER
CREATE TABLE [USER] (
    userID INT PRIMARY KEY IDENTITY(1,1),
    username VARCHAR(255) UNIQUE NOT NULL,
    email VARCHAR(255) NOT NULL,
    dateJoined DATETIME NOT NULL,
    isActive BIT NOT NULL,
	profilePicture NVARCHAR(MAX),
	subscriptionPlan NVARCHAR(MAX),
	hashedPassword NVARCHAR(MAX),
	salt CHAR(24)
);

-- ARTIST
CREATE TABLE ARTIST (
    artistID INT PRIMARY KEY IDENTITY(1,1),
    artistName VARCHAR(255) NOT NULL,
    biography TEXT,
    genre VARCHAR(255) NOT NULL,
	artistPicture NVARCHAR(MAX)
);


-- Albums
CREATE TABLE ALBUM (
    albumID INT PRIMARY KEY IDENTITY(1,1),
    artistID INT FOREIGN KEY REFERENCES ARTIST(artistID),
    albumTitle VARCHAR(255) NOT NULL,
    albumReleaseDate DATETIME NOT NULL,
    albumPicture VARCHAR(255) NOT NULL,
	albumArtist INT,
	albumGenre NVARCHAR(MAX),
	FOREIGN KEY (albumArtist) REFERENCES ARTIST(artistID)
);

-- Songs
CREATE TABLE SONG (
    songID INT PRIMARY KEY IDENTITY(1,1),
    songTitle VARCHAR(255) NOT NULL,
    artistID INT FOREIGN KEY REFERENCES ARTIST(artistID),
    albumID INT FOREIGN KEY REFERENCES ALBUM(albumID),
    songGenre VARCHAR(255) NOT NULL,
    songDuration TIME NOT NULL,
    songReleaseDate DATETIME NOT NULL
);

-- Playlists
CREATE TABLE PLAYLIST (
    playlistID INT PRIMARY KEY IDENTITY(1,1),
    ownerID INT FOREIGN KEY REFERENCES [USER](userID),
    playlistName VARCHAR(255) NOT NULL,
    playlistDescription TEXT,
    playlistCreationDate DATETIME NOT NULL
);

-- PlaylistSongs
CREATE TABLE PLAYLISTSONG (
    playlistID INT,
    songID INT,
    PRIMARY KEY (playlistID, songID),
    FOREIGN KEY (playlistID) REFERENCES PLAYLIST(playlistID),
    FOREIGN KEY (songID) REFERENCES SONG(songID)
);

-- Feedback
CREATE TABLE FEEDBACK (
    feedbackID INT PRIMARY KEY IDENTITY(1,1),
    userID INT FOREIGN KEY REFERENCES [USER](userID),
    songID INT FOREIGN KEY REFERENCES SONG(songID),
    rating INT NOT NULL,
    Comment TEXT,
    feedbackDate DATETIME NOT NULL
);

-- Events
CREATE TABLE [EVENT] (
    eventID INT PRIMARY KEY IDENTITY(1,1),
    artistID INT FOREIGN KEY REFERENCES ARTIST(artistID),
    eventName VARCHAR(255) NOT NULL,
    eventDescription TEXT,
    eventLocation VARCHAR(255) NOT NULL,
    eventDate DATETIME NOT NULL,
    eventTicketPrice DECIMAL(10, 2) NOT NULL
);

-- Tickets
CREATE TABLE TICKET (
    ticketID INT PRIMARY KEY IDENTITY(1,1),
    eventID INT FOREIGN KEY REFERENCES EVENT(eventID),
    userID INT FOREIGN KEY REFERENCES [USER](userID),
    purchaseDate DATETIME NOT NULL,
    ticketPrice DECIMAL(10, 2) NOT NULL,
	ticketType VARCHAR(50)
);
