export enum SelectedOutcome {
  HomeWin = 0,
  AwayWin = 1,
  Draw = 2,
}

export interface PredictionEntry {
  matchId: string;
  selectedOutcome: SelectedOutcome;
}

export interface ParticipateRequest {
  displayName: string;
  predictions: PredictionEntry[];
}
