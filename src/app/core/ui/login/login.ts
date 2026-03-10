import { Component, inject } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { AsyncPipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-login',
  imports: [AsyncPipe, MatButtonModule, MatCardModule, MatProgressSpinnerModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class LoginComponent {
  protected auth = inject(AuthService);

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
