import { Component, input } from '@angular/core';

@Component({
  selector: 'app-round-list',
  templateUrl: './round-list.html',
  styleUrl: './round-list.scss',
})
export class RoundListComponent {
  boardId = input.required<string>();
}
