import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-board-detail',
  imports: [RouterLink],
  templateUrl: './board-detail.html',
  styleUrl: './board-detail.scss',
})
export class BoardDetailComponent {
  boardId = input.required<string>();
}
