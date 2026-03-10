import { Component, input } from '@angular/core';

@Component({
  selector: 'app-admin-round',
  templateUrl: './admin-round.html',
  styleUrl: './admin-round.scss',
})
export class AdminRoundComponent {
  boardId = input.required<string>();
  roundId = input.required<string>();
}
