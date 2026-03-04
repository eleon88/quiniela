import { Component, input } from '@angular/core';

@Component({
  selector: 'app-admin-board',
  standalone: true,
  templateUrl: './admin-board.html',
  styleUrl: './admin-board.scss',
})
export class AdminBoardComponent {
  boardId = input.required<string>();
}
