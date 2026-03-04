import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-round-detail',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './round-detail.html',
  styleUrl: './round-detail.scss',
})
export class RoundDetailComponent {
  boardId = input.required<string>();
  roundId = input.required<string>();
}
