import { Component, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatIconModule } from '@angular/material/icon';
import { LeaderboardService } from '../../services/leaderboard.service';
import { LoadingComponent } from '../../../../shared/components/loading/loading';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message';

@Component({
  selector: 'app-leaderboard',
  imports: [RouterLink, MatIconModule, LoadingComponent, ErrorMessageComponent],
  templateUrl: './leaderboard.html',
  styleUrl: './leaderboard.scss',
})
export class LeaderboardComponent {
  boardId = input.required<string>();
  roundId = input.required<string>();

  private leaderboardService = inject(LeaderboardService);

  leaderboard = rxResource({
    params: () => this.roundId(),
    stream: ({ params: id }) => this.leaderboardService.getLeaderboard(id),
  });
}
