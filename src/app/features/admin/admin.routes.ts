import { Routes } from '@angular/router';
import { AdminBoardComponent } from './ui/admin-board/admin-board';
import { AdminRoundComponent } from './ui/admin-round/admin-round';

export const adminRoutes: Routes = [
  { path: 'boards/:boardId', component: AdminBoardComponent },
  { path: 'boards/:boardId/rounds/:roundId', component: AdminRoundComponent },
];
