import { Routes } from '@angular/router';
import { AdminDashboardComponent } from './ui/admin-dashboard/admin-dashboard';
import { AdminBoardComponent } from './ui/admin-board/admin-board';
import { AdminRoundComponent } from './ui/admin-round/admin-round';

export const adminRoutes: Routes = [
  { path: '', component: AdminDashboardComponent },
  { path: 'boards/:boardId', component: AdminBoardComponent },
  { path: 'boards/:boardId/rounds/:roundId', component: AdminRoundComponent },
];
