import { Routes } from '@angular/router';
import { BoardListComponent } from './ui/board-list/board-list';
import { BoardDetailComponent } from './ui/board-detail/board-detail';

export const boardsRoutes: Routes = [
  { path: '', component: BoardListComponent },
  { path: ':boardId', component: BoardDetailComponent },
  {
    path: ':boardId/rounds',
    loadChildren: () => import('../rounds/rounds.routes').then(m => m.roundsRoutes),
  },
];
