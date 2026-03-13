import { Component, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { BoardsService } from '../../services/boards.service';
import { LoadingComponent } from '../../../../shared/components/loading/loading';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message';

@Component({
  selector: 'app-board-detail',
  imports: [RouterLink, MatCardModule, MatButtonModule, MatIconModule, LoadingComponent, ErrorMessageComponent],
  templateUrl: './board-detail.html',
  styleUrl: './board-detail.scss',
})
export class BoardDetailComponent {
  boardId = input.required<string>();

  private boardsService = inject(BoardsService);

  board = rxResource({
    params: () => this.boardId(),
    stream: ({ params: id }) => this.boardsService.getBoard(id),
  });
}
