-- =============================================
-- Quiniela Platform - Seed Data
-- =============================================

USE Quiniela;
GO

-- =============================================
-- Users
-- =============================================

DECLARE @AdminId  UNIQUEIDENTIFIER = 'A0000000-0000-0000-0000-000000000001';
DECLARE @User2Id  UNIQUEIDENTIFIER = 'A0000000-0000-0000-0000-000000000002';
DECLARE @User3Id  UNIQUEIDENTIFIER = 'A0000000-0000-0000-0000-000000000003';
DECLARE @User4Id  UNIQUEIDENTIFIER = 'A0000000-0000-0000-0000-000000000004';

INSERT INTO [User] (Id, Auth0Id, Email, DisplayName, IsPlatformAdmin) VALUES
  (@AdminId, 'auth0|admin001', 'admin@quiniela.dev',  'Admin Carlos', 1),
  (@User2Id, 'auth0|user002',  'maria@example.com',   'Maria Lopez',  0),
  (@User3Id, 'auth0|user003',  'jorge@example.com',   'Jorge Ramirez',0),
  (@User4Id, 'auth0|user004',  'ana@example.com',     'Ana Torres',   0);

-- =============================================
-- Boards
-- =============================================

DECLARE @Board1Id UNIQUEIDENTIFIER = 'B0000000-0000-0000-0000-000000000001';
DECLARE @Board2Id UNIQUEIDENTIFIER = 'B0000000-0000-0000-0000-000000000002';

INSERT INTO Board (Id, Name, Description, OwnerUserId, IsPublic, IsPremium) VALUES
  (@Board1Id, 'Liga MX Clausura 2026',  'Public board for Liga MX fans',         @AdminId, 1, 0),
  (@Board2Id, 'Office Pool - Devs Only', 'Private board for the dev team',        @User2Id, 0, 0);

-- =============================================
-- Board Admins
-- =============================================

INSERT INTO BoardAdmin (BoardId, UserId, Role) VALUES
  (@Board1Id, @AdminId, 0), -- Owner
  (@Board1Id, @User2Id, 1), -- Admin
  (@Board2Id, @User2Id, 0), -- Owner
  (@Board2Id, @User3Id, 2); -- Moderator

-- =============================================
-- Rounds
-- =============================================

DECLARE @Round1Id UNIQUEIDENTIFIER = 'C0000000-0000-0000-0000-000000000001'; -- Completed
DECLARE @Round2Id UNIQUEIDENTIFIER = 'C0000000-0000-0000-0000-000000000002'; -- Open
DECLARE @Round3Id UNIQUEIDENTIFIER = 'C0000000-0000-0000-0000-000000000003'; -- Draft

INSERT INTO Round (Id, BoardId, Name, Status, StartDateTime, EndDateTime) VALUES
  (@Round1Id, @Board1Id, 'Jornada 8',  3, '2026-02-14 19:00:00', '2026-02-16 21:00:00'), -- Completed
  (@Round2Id, @Board1Id, 'Jornada 9',  1, '2026-03-07 19:00:00', '2026-03-09 21:00:00'), -- Open
  (@Round3Id, @Board2Id, 'Week 1',     0, NULL, NULL);                                    -- Draft

-- =============================================
-- Matches - Jornada 8 (Completed)
-- =============================================

DECLARE @M1 UNIQUEIDENTIFIER = 'D0000000-0000-0000-0000-000000000001';
DECLARE @M2 UNIQUEIDENTIFIER = 'D0000000-0000-0000-0000-000000000002';
DECLARE @M3 UNIQUEIDENTIFIER = 'D0000000-0000-0000-0000-000000000003';

INSERT INTO Match (Id, RoundId, HomeTeam, AwayTeam, StartDateTime, Result, IsLocked) VALUES
  (@M1, @Round1Id, 'Club America',  'Guadalajara',  '2026-02-14 21:00:00', 0, 1), -- HomeWin
  (@M2, @Round1Id, 'Cruz Azul',     'Pumas UNAM',   '2026-02-15 19:00:00', 2, 1), -- Draw
  (@M3, @Round1Id, 'Monterrey',     'Tigres UANL',  '2026-02-16 20:00:00', 1, 1); -- AwayWin

-- =============================================
-- Matches - Jornada 9 (Open, pending results)
-- =============================================

DECLARE @M4 UNIQUEIDENTIFIER = 'D0000000-0000-0000-0000-000000000004';
DECLARE @M5 UNIQUEIDENTIFIER = 'D0000000-0000-0000-0000-000000000005';
DECLARE @M6 UNIQUEIDENTIFIER = 'D0000000-0000-0000-0000-000000000006';

INSERT INTO Match (Id, RoundId, HomeTeam, AwayTeam, StartDateTime, Result, IsLocked) VALUES
  (@M4, @Round2Id, 'Guadalajara',  'Cruz Azul',    '2026-03-07 19:00:00', 3, 0), -- Pending
  (@M5, @Round2Id, 'Tigres UANL',  'Club America', '2026-03-08 21:00:00', 3, 0), -- Pending
  (@M6, @Round2Id, 'Pumas UNAM',   'Monterrey',    '2026-03-09 17:00:00', 3, 0); -- Pending

-- =============================================
-- Participants - Jornada 8 (scored)
-- =============================================

DECLARE @P1 UNIQUEIDENTIFIER = 'E0000000-0000-0000-0000-000000000001';
DECLARE @P2 UNIQUEIDENTIFIER = 'E0000000-0000-0000-0000-000000000002';
DECLARE @P3 UNIQUEIDENTIFIER = 'E0000000-0000-0000-0000-000000000003';
DECLARE @P4 UNIQUEIDENTIFIER = 'E0000000-0000-0000-0000-000000000004'; -- anonymous

INSERT INTO Participant (Id, RoundId, UserId, DisplayName, IsActive, Score) VALUES
  (@P1, @Round1Id, @User2Id, 'Maria Lopez',   1, 2),
  (@P2, @Round1Id, @User3Id, 'Jorge Ramirez', 1, 3),
  (@P3, @Round1Id, @User4Id, 'Ana Torres',    1, 1),
  (@P4, @Round1Id, NULL,     'Guest Pedro',   1, 2); -- anonymous participant

-- =============================================
-- Predictions - Jornada 8
-- =============================================

-- Maria (2 pts): HomeWin ✓, HomeWin ✗(Draw), Draw ✗(AwayWin)
INSERT INTO Prediction (ParticipantId, MatchId, SelectedOutcome) VALUES
  (@P1, @M1, 0), -- HomeWin ✓
  (@P1, @M2, 0), -- HomeWin ✗
  (@P1, @M3, 2); -- Draw ✗

-- Jorge (3 pts): HomeWin ✓, Draw ✓, AwayWin ✓ — perfect round!
INSERT INTO Prediction (ParticipantId, MatchId, SelectedOutcome) VALUES
  (@P2, @M1, 0), -- HomeWin ✓
  (@P2, @M2, 2), -- Draw ✓
  (@P2, @M3, 1); -- AwayWin ✓

-- Ana (1 pt): AwayWin ✗, Draw ✓, HomeWin ✗
INSERT INTO Prediction (ParticipantId, MatchId, SelectedOutcome) VALUES
  (@P3, @M1, 1), -- AwayWin ✗
  (@P3, @M2, 2), -- Draw ✓
  (@P3, @M3, 0); -- HomeWin ✗

-- Guest Pedro (2 pts): HomeWin ✓, AwayWin ✗, AwayWin ✓
INSERT INTO Prediction (ParticipantId, MatchId, SelectedOutcome) VALUES
  (@P4, @M1, 0), -- HomeWin ✓
  (@P4, @M2, 1), -- AwayWin ✗
  (@P4, @M3, 1); -- AwayWin ✓

-- =============================================
-- Participants - Jornada 9 (open, accepting predictions)
-- =============================================

DECLARE @P5 UNIQUEIDENTIFIER = 'E0000000-0000-0000-0000-000000000005';
DECLARE @P6 UNIQUEIDENTIFIER = 'E0000000-0000-0000-0000-000000000006';
DECLARE @P7 UNIQUEIDENTIFIER = 'E0000000-0000-0000-0000-000000000007'; -- inactive, waiting activation

INSERT INTO Participant (Id, RoundId, UserId, DisplayName, IsActive, Score) VALUES
  (@P5, @Round2Id, @User2Id, 'Maria Lopez',   1, 0),
  (@P6, @Round2Id, @User3Id, 'Jorge Ramirez', 1, 0),
  (@P7, @Round2Id, NULL,     'Guest Luis',    0, 0); -- pending activation

-- =============================================
-- Predictions - Jornada 9
-- =============================================

-- Maria's predictions for Jornada 9
INSERT INTO Prediction (ParticipantId, MatchId, SelectedOutcome) VALUES
  (@P5, @M4, 0), -- HomeWin
  (@P5, @M5, 1), -- AwayWin
  (@P5, @M6, 2); -- Draw

-- Jorge's predictions for Jornada 9
INSERT INTO Prediction (ParticipantId, MatchId, SelectedOutcome) VALUES
  (@P6, @M4, 2), -- Draw
  (@P6, @M5, 0), -- HomeWin
  (@P6, @M6, 0); -- HomeWin

GO
