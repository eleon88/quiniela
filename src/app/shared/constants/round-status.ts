import { RoundStatus } from '../../features/rounds/models/round';
import { MatchResult } from '../../features/rounds/models/match';

export const STATUS_LABEL: Record<RoundStatus, string> = {
  [RoundStatus.Draft]: 'Draft',
  [RoundStatus.Open]: 'Open',
  [RoundStatus.Active]: 'Active',
  [RoundStatus.Completed]: 'Completed',
};

export const STATUS_CLASS: Record<RoundStatus, string> = {
  [RoundStatus.Draft]: 'status-draft',
  [RoundStatus.Open]: 'status-open',
  [RoundStatus.Active]: 'status-active',
  [RoundStatus.Completed]: 'status-completed',
};

export const RESULT_LABEL: Record<MatchResult, string> = {
  [MatchResult.Pending]: 'Pending',
  [MatchResult.HomeWin]: 'Home Win',
  [MatchResult.AwayWin]: 'Away Win',
  [MatchResult.Draw]: 'Draw',
};

export const RESULT_CLASS: Record<MatchResult, string> = {
  [MatchResult.Pending]: 'result-pending',
  [MatchResult.HomeWin]: 'result-home',
  [MatchResult.AwayWin]: 'result-away',
  [MatchResult.Draw]: 'result-draw',
};
