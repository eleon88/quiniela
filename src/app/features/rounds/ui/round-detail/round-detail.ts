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
import { STATUS_LABEL, STATUS_CLASS, RESULT_LABEL, RESULT_CLASS } from '../../../../shared/constants/round-status';

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
    params: () => this.roundId(),
    stream: ({ params: id }) => this.roundsService.getRound(id),
  });

  matches = rxResource({
    params: () => this.roundId(),
    stream: ({ params: id }) => this.roundsService.getMatches(id),
  });

  readonly RoundStatus = RoundStatus;
  readonly MatchResult = MatchResult;

  readonly statusLabel = STATUS_LABEL;
  readonly statusClass = STATUS_CLASS;
  readonly resultLabel = RESULT_LABEL;
  readonly resultClass = RESULT_CLASS;
}
