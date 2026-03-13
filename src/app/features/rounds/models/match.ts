export enum MatchResult {
  HomeWin = 0,
  AwayWin = 1,
  Draw = 2,
  Pending = 3,
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
