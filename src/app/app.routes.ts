import { Routes } from '@angular/router';

export const routes: Routes = [
    {path: 'users', loadChildren: () => import('./features/users/users.routes').then(m => m.routes)},
    { path: 'quiniela', loadChildren: () => import('./features/quiniela/quiniela.routes').then(m => m.routes)},
    { path: '', redirectTo: '/quiniela', pathMatch: 'full' },
    { path: 'games', loadChildren: () => import('./features/games/games.routes').then(m => m.routes) }
];
