import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { BoardsService } from '../../services/boards.service';
import { LoadingComponent } from '../../../../shared/components/loading/loading';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message';

@Component({
  selector: 'app-board-list',
  templateUrl: './board-list.html',
  styleUrl: './board-list.scss',
  imports: [RouterLink, MatCardModule, LoadingComponent, ErrorMessageComponent],
})
export class BoardListComponent {
  private boardsService = inject(BoardsService);

  boards = rxResource({
    stream: () => this.boardsService.getBoards(),
  });
}
