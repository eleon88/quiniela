export enum MatchResult {
  Pending = 0,
  HomeWin = 1,
  AwayWin = 2,
  Draw = 3,
}

export interface Match {
  id: string;
  roundId: string;
  homeTeam: string;
  awayTeam: string;
  startDateTime: string;
  result: MatchResult;
  isLocked: boolean;
}
