import { Routes } from '@angular/router';
import { RoundListComponent } from './ui/round-list/round-list';
import { RoundDetailComponent } from './ui/round-detail/round-detail';

export const roundsRoutes: Routes = [
  { path: '', component: RoundListComponent },
  { path: ':roundId', component: RoundDetailComponent },
  {
    path: ':roundId/leaderboard',
    loadChildren: () => import('../leaderboard/leaderboard.routes').then(m => m.leaderboardRoutes),
  },
  {
    path: ':roundId/predict',
    loadChildren: () => import('../predictions/predictions.routes').then(m => m.predictionsRoutes),
  },
];
