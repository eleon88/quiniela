import { Routes } from '@angular/router';
import { LayoutComponent } from './core/layout/layout';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: 'boards', pathMatch: 'full' },
      {
        path: 'boards',
        loadChildren: () => import('./features/boards/boards.routes').then(m => m.boardsRoutes),
      },
      {
        path: 'admin',
        loadChildren: () => import('./features/admin/admin.routes').then(m => m.adminRoutes),
      },
      {
        path: 'login',
        loadComponent: () => import('./core/ui/login/login').then(m => m.LoginComponent),
      },
    ],
  },
  { path: '**', loadComponent: () => import('./shared/components/page-not-found/page-not-found').then(m => m.PageNotFoundComponent) },
];
