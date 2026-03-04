import { Component, input } from '@angular/core';

@Component({
  selector: 'app-admin-round',
  standalone: true,
  templateUrl: './admin-round.html',
  styleUrl: './admin-round.scss',
})
export class AdminRoundComponent {
  boardId = input.required<string>();
  roundId = input.required<string>();
}
