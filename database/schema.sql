-- =============================================
-- Quiniela Platform - Database Schema
-- Target: MS SQL Server
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Quiniela')
  CREATE DATABASE Quiniela;
GO

USE Quiniela;
GO

-- =============================================
-- Tables
-- =============================================

CREATE TABLE [User] (
  Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  Auth0Id         NVARCHAR(128)    NOT NULL,
  Email           NVARCHAR(256)    NOT NULL,
  DisplayName     NVARCHAR(128)    NOT NULL,
  CreatedAt       DATETIME2        NOT NULL DEFAULT SYSUTCDATETIME(),
  IsPlatformAdmin BIT              NOT NULL DEFAULT 0,

  CONSTRAINT PK_User PRIMARY KEY (Id),
  CONSTRAINT UQ_User_Auth0Id UNIQUE (Auth0Id),
  CONSTRAINT UQ_User_Email UNIQUE (Email)
);

CREATE TABLE Board (
  Id          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  Name        NVARCHAR(256)    NOT NULL,
  Description NVARCHAR(1000)   NULL,
  OwnerUserId UNIQUEIDENTIFIER NOT NULL,
  IsPublic    BIT              NOT NULL DEFAULT 1,
  IsPremium   BIT              NOT NULL DEFAULT 0,
  CreatedAt   DATETIME2        NOT NULL DEFAULT SYSUTCDATETIME(),

  CONSTRAINT PK_Board PRIMARY KEY (Id),
  CONSTRAINT FK_Board_Owner FOREIGN KEY (OwnerUserId) REFERENCES [User](Id)
);

CREATE TABLE BoardAdmin (
  BoardId UNIQUEIDENTIFIER NOT NULL,
  UserId  UNIQUEIDENTIFIER NOT NULL,
  Role    TINYINT          NOT NULL, -- 0=Owner, 1=Admin, 2=Moderator

  CONSTRAINT PK_BoardAdmin PRIMARY KEY (BoardId, UserId),
  CONSTRAINT FK_BoardAdmin_Board FOREIGN KEY (BoardId) REFERENCES Board(Id),
  CONSTRAINT FK_BoardAdmin_User FOREIGN KEY (UserId) REFERENCES [User](Id),
  CONSTRAINT CK_BoardAdmin_Role CHECK (Role IN (0, 1, 2))
);

CREATE TABLE Round (
  Id            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  BoardId       UNIQUEIDENTIFIER NOT NULL,
  Name          NVARCHAR(256)    NOT NULL,
  Status        TINYINT          NOT NULL DEFAULT 0, -- 0=Draft, 1=Open, 2=Active, 3=Completed
  StartDateTime DATETIME2        NULL,
  EndDateTime   DATETIME2        NULL,
  CreatedAt     DATETIME2        NOT NULL DEFAULT SYSUTCDATETIME(),

  CONSTRAINT PK_Round PRIMARY KEY (Id),
  CONSTRAINT FK_Round_Board FOREIGN KEY (BoardId) REFERENCES Board(Id),
  CONSTRAINT CK_Round_Status CHECK (Status IN (0, 1, 2, 3))
);

CREATE TABLE Match (
  Id            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  RoundId       UNIQUEIDENTIFIER NOT NULL,
  HomeTeam      NVARCHAR(128)    NOT NULL,
  AwayTeam      NVARCHAR(128)    NOT NULL,
  StartDateTime DATETIME2        NOT NULL,
  Result        TINYINT          NOT NULL DEFAULT 3, -- 0=HomeWin, 1=AwayWin, 2=Draw, 3=Pending
  IsLocked      BIT              NOT NULL DEFAULT 0,

  CONSTRAINT PK_Match PRIMARY KEY (Id),
  CONSTRAINT FK_Match_Round FOREIGN KEY (RoundId) REFERENCES Round(Id),
  CONSTRAINT CK_Match_Result CHECK (Result IN (0, 1, 2, 3))
);

CREATE TABLE Participant (
  Id          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  RoundId     UNIQUEIDENTIFIER NOT NULL,
  UserId      UNIQUEIDENTIFIER NULL,
  DisplayName NVARCHAR(128)    NOT NULL,
  IsActive    BIT              NOT NULL DEFAULT 0,
  Score       INT              NOT NULL DEFAULT 0,
  CreatedAt   DATETIME2        NOT NULL DEFAULT SYSUTCDATETIME(),

  CONSTRAINT PK_Participant PRIMARY KEY (Id),
  CONSTRAINT FK_Participant_Round FOREIGN KEY (RoundId) REFERENCES Round(Id),
  CONSTRAINT FK_Participant_User FOREIGN KEY (UserId) REFERENCES [User](Id)
);

CREATE TABLE Prediction (
  Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
  ParticipantId   UNIQUEIDENTIFIER NOT NULL,
  MatchId         UNIQUEIDENTIFIER NOT NULL,
  SelectedOutcome TINYINT          NOT NULL, -- 0=HomeWin, 1=AwayWin, 2=Draw

  CONSTRAINT PK_Prediction PRIMARY KEY (Id),
  CONSTRAINT FK_Prediction_Participant FOREIGN KEY (ParticipantId) REFERENCES Participant(Id),
  CONSTRAINT FK_Prediction_Match FOREIGN KEY (MatchId) REFERENCES Match(Id),
  CONSTRAINT CK_Prediction_Outcome CHECK (SelectedOutcome IN (0, 1, 2))
);

-- =============================================
-- Indexes (from SPEC.MD section 11)
-- =============================================

CREATE UNIQUE INDEX IX_Participant_Round_DisplayName
  ON Participant (RoundId, DisplayName);

CREATE UNIQUE INDEX IX_Prediction_Participant_Match
  ON Prediction (ParticipantId, MatchId);

CREATE INDEX IX_Match_Round
  ON Match (RoundId);

CREATE INDEX IX_Participant_Round_Score
  ON Participant (RoundId, Score DESC, DisplayName ASC);
GO
