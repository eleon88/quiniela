import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '@auth0/auth0-angular';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-login',
  imports: [MatButtonModule, MatCardModule, MatProgressSpinnerModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class LoginComponent {
  private auth = inject(AuthService);

  isLoading = toSignal(this.auth.isLoading$, { initialValue: true });
  isAuthenticated = toSignal(this.auth.isAuthenticated$, { initialValue: false });
  user = toSignal(this.auth.user$);
  error = toSignal(this.auth.error$);

  login(): void {
    this.auth.loginWithRedirect();
  }

  signup(): void {
    this.auth.loginWithRedirect({ authorizationParams: { screen_hint: 'signup' } });
  }

  logout(): void {
    this.auth.logout({ logoutParams: { returnTo: window.location.origin } });
  }
}
