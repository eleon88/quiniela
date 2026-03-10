import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-page-not-found',
  imports: [RouterLink],
  template: `
    <div class="container text-center mt-5">
      <h1>404</h1>
      <p>Page not found</p>
      <a routerLink="/boards">Go to Boards</a>
    </div>
  `,
})
export class PageNotFoundComponent {}
