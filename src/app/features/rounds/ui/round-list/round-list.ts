import { Component, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RoundsService } from '../../services/rounds.service';
import { RoundStatus } from '../../models/round';
import { LoadingComponent } from '../../../../shared/components/loading/loading';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message';

@Component({
  selector: 'app-round-list',
  imports: [RouterLink, DatePipe, MatCardModule, MatIconModule, LoadingComponent, ErrorMessageComponent],
  templateUrl: './round-list.html',
  styleUrl: './round-list.scss',
})
export class RoundListComponent {
  boardId = input.required<string>();

  private roundsService = inject(RoundsService);

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

  rounds = rxResource({
    params: () => this.boardId(),
    stream: ({ params: id }) => this.roundsService.getRounds(id),
  });
}
