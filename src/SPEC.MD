# Quiniela Platform Specification

## 1. High-Level Architecture

### Stack

| Layer | Technology |
|-------|-----------|
| Frontend | Angular (latest, standalone components, signals, zoneless) |
| Backend | .NET 8 Web API |
| Data Access | Dapper |
| Database | MS SQL Server |
| Authentication | Auth0 (`@auth0/auth0-angular`) |
| Realtime | SignalR |
| Source Control | Git |
| Styling | SCSS + Bootstrap 5 + Angular Material |

## 2. Core Domain Model

Because we want:

- Multiple quinielas at same time
- Admin-created boards (public/private)
- Role-based management
- Premium tier possibility

We must separate:

**Platform → Boards → Rounds → Matches → Predictions → Participants**

## 3. Core Entities

### 3.1 User

Represents an authenticated Auth0 user.

| Field | Type |
|-------|------|
| Id | GUID |
| Auth0Id | string |
| Email | string |
| DisplayName | string |
| CreatedAt | datetime |
| IsPlatformAdmin | bit |

> Anonymous users are NOT stored as Users.

### 3.2 Board (Quiniela Board)

This is the monetizable unit.

Examples: "Liga MX Fans Board", "Office Pool", "Friends Private League"

| Field | Type |
|-------|------|
| Id | GUID |
| Name | string |
| Description | string |
| OwnerUserId | FK User |
| IsPublic | bit |
| IsPremium | bit |
| CreatedAt | datetime |

### 3.3 BoardAdmin

Allows role-based access.

| Field | Type |
|-------|------|
| BoardId | FK |
| UserId | FK |
| Role | enum: Owner, Admin, Moderator |

### 3.4 Round

Each board has multiple rounds.

| Field | Type |
|-------|------|
| Id | GUID |
| BoardId | FK |
| Name | string (e.g. "Liga MX Round 9") |
| Status | enum: Draft, Open, Active, Completed |
| StartDateTime | datetime |
| EndDateTime | datetime |
| CreatedAt | datetime |

#### Round Status Rules

| Status | Meaning |
|--------|---------|
| Draft | Admin editing |
| Open | Users can submit |
| Active | First match started |
| Completed | All matches finalized |

### 3.5 Match

| Field | Type |
|-------|------|
| Id | GUID |
| RoundId | FK |
| HomeTeam | string |
| AwayTeam | string |
| StartDateTime | datetime |
| Result | enum: HomeWin, AwayWin, Draw, Pending |
| IsLocked | bit |

### 3.6 Participant

Represents a person participating in a specific round.

> Names must be unique per board per round.

| Field | Type |
|-------|------|
| Id | GUID |
| RoundId | FK |
| UserId | nullable FK |
| DisplayName | string |
| IsActive | bit |
| Score | int |
| CreatedAt | datetime |

**Rules:**

- If `UserId` is NULL → anonymous
- Unique index: `(RoundId + DisplayName)`

### 3.7 Prediction

Users can submit multiple entries. One Participant = One submission. If they submit again, they create a new Participant entry.

| Field | Type |
|-------|------|
| Id | GUID |
| ParticipantId | FK |
| MatchId | FK |
| SelectedOutcome | enum: HomeWin, AwayWin, Draw |

Composite unique index: `(ParticipantId + MatchId)`

## 4. Business Rules

### 4.1 Submission Rules

- Round must be **Open**
- Once Round becomes **Active**:
  - No new Participants
  - No new Predictions
  - Inactive list disappears

### 4.2 Activation Flow

When user submits:

1. Participant created with `IsActive = false`
2. Admin must manually activate

When first match starts:

1. Round status automatically becomes **Active**
2. All inactive participants are ignored

### 4.3 Scoring Logic

- **1 point** per correct outcome
- When a Match result is updated:
  - System recalculates scores for that Round
  - Leaderboard updates
  - SignalR broadcast triggers
- Sort: `ORDER BY Score DESC, DisplayName ASC`

## 5. SignalR Real-Time Architecture

### Hub

`LeaderboardHub`

### Events

- `MatchResultUpdated`
- `RoundStatusChanged`
- `LeaderboardUpdated`

### Clients

- Join group per `RoundId`
- Receive leaderboard updates instantly

## 6. API Design

### Public Endpoints

```
GET    /api/boards                        (public only)
GET    /api/boards/{id}
GET    /api/rounds/{boardId}
GET    /api/round/{roundId}/matches
GET    /api/round/{roundId}/leaderboard
POST   /api/round/{roundId}/participate
```

### Authenticated Endpoints

```
POST   /api/boards
POST   /api/boards/{id}/round
POST   /api/round/{id}/matches
PUT    /api/round/{id}/status
PUT    /api/match/{id}/result
PUT    /api/participant/{id}/activate
```

### Admin-Only (Board Scoped)

Authorize by: Owner, Admin, Moderator

## 7. Angular Architecture

```
core/
  auth/
  interceptors/
  signalr/

shared/
  models/
  ui/
  pipes/

features/
  boards/
  rounds/
  leaderboard/
  predictions/
  admin/
```

**Use:**

- Signals for state
- Feature-level state stores
- No NgRx needed
- Signal-based services

## 8. Auth Strategy

### Public

- Can view public boards
- Can participate anonymously

### Authenticated

- Can create boards
- Can become admins
- Can manage boards

### Role-based Access

JWT claims from Auth0: `roles`, `permissions`

## 9. Monetization Tier Structure (Future-Proof)

| Tier | Features |
|------|----------|
| Free | Can only participate, no boards |
| Premium | Up to 50 boards, private boards, add 2 Moderators |
| Pro | Add Admins, custom scoring rules |

## 10. API-Based Match Data Strategy

Enter games manually or pull from TheSportsDB free tier.

## 11. Database Index Strategy

Critical indexes:

- `IX_Participant_Round_DisplayName` (unique)
- `IX_Prediction_Participant_Match` (unique)
- `IX_Match_Round`
- `IX_Participant_Round_Score`

## 12. Concurrency Protection

- SQL transactions for match result update
- Recalculate scores inside transaction
- Prevent race conditions

## 13. Edge Cases Covered

- Duplicate names
- Late submissions
- Anonymous edit prevention
- Multiple entries per user
- Role separation
- Private board access
- SignalR sync

## 14. Future Enhancements

- Tie-breaker question
- Exact score prediction
- AI prediction assistant (OpenAI integration)
- Historical analytics
- Player statistics
- PWA install support

## 15. Architecture Strength

- Multi-tenant SaaS architecture
- Role-based access
- Realtime system
- Scalable board isolation
- Monetization-ready

> This is portfolio-level SaaS engineering.
