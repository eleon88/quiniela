import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '@auth0/auth0-angular';
import { filter, distinctUntilChanged, switchMap, take } from 'rxjs';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './layout.html',
  styleUrl: './layout.scss',
})
export class LayoutComponent {
  private auth = inject(AuthService);
  private usersService = inject(UsersService);

  isAuthenticated = toSignal(this.auth.isAuthenticated$, { initialValue: false });
  user = toSignal(this.auth.user$);

  constructor() {
    this.auth.isAuthenticated$.pipe(
      distinctUntilChanged(),
      filter(isAuth => isAuth),
      switchMap(() => this.auth.user$.pipe(filter(u => !!u), take(1))),
      switchMap(user => this.usersService.syncCurrentUser(
        user?.email ?? '',
        user?.name ?? user?.email ?? '',
      )),
      takeUntilDestroyed(),
    ).subscribe({
      error: (err) => console.error('User sync failed:', err),
    });
  }

  login(): void {
    this.auth.loginWithRedirect();
  }

  logout(): void {
    this.auth.logout({ logoutParams: { returnTo: window.location.origin } });
  }
}
