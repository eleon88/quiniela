import { Component, input } from '@angular/core';

@Component({
  selector: 'app-leaderboard',
  templateUrl: './leaderboard.html',
  styleUrl: './leaderboard.scss',
})
export class LeaderboardComponent {
  boardId = input.required<string>();
  roundId = input.required<string>();
}
