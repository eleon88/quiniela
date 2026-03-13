import { Component, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RoundsService } from '../../services/rounds.service';
import { RoundStatus } from '../../models/round';
import { MatchResult } from '../../models/match';
import { LoadingComponent } from '../../../../shared/components/loading/loading';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message';

@Component({
  selector: 'app-round-detail',
  imports: [RouterLink, DatePipe, MatButtonModule, MatIconModule, LoadingComponent, ErrorMessageComponent],
  templateUrl: './round-detail.html',
  styleUrl: './round-detail.scss',
})
export class RoundDetailComponent {
  boardId = input.required<string>();
  roundId = input.required<string>();

  private roundsService = inject(RoundsService);

  round = rxResource({
    params: () => ({ boardId: this.boardId(), roundId: this.roundId() }),
    stream: ({ params }) => this.roundsService.getRound(params.boardId, params.roundId),
  });

  matches = rxResource({
    params: () => this.roundId(),
    stream: ({ params: id }) => this.roundsService.getMatches(id),
  });

  readonly RoundStatus = RoundStatus;
  readonly MatchResult = MatchResult;

  readonly statusLabel: Record<RoundStatus, string> = {
    [RoundStatus.Draft]: 'Draft',
    [RoundStatus.Open]: 'Open',
    [RoundStatus.Active]: 'Active',
    [RoundStatus.Completed]: 'Completed',
  };

  readonly statusClass: Record<RoundStatus, string> = {
    [RoundStatus.Draft]: 'status-draft',
    [RoundStatus.Open]: 'status-open',
    [RoundStatus.Active]: 'status-active',
    [RoundStatus.Completed]: 'status-completed',
  };

  readonly resultLabel: Record<MatchResult, string> = {
    [MatchResult.Pending]: 'Pending',
    [MatchResult.HomeWin]: 'Home Win',
    [MatchResult.AwayWin]: 'Away Win',
    [MatchResult.Draw]: 'Draw',
  };

  readonly resultClass: Record<MatchResult, string> = {
    [MatchResult.Pending]: 'result-pending',
    [MatchResult.HomeWin]: 'result-home',
    [MatchResult.AwayWin]: 'result-away',
    [MatchResult.Draw]: 'result-draw',
  };
}
