import { Component } from '@angular/core';

@Component({
  selector: 'app-page-not-found',
  standalone: true,
  template: `
    <div class="container text-center mt-5">
      <h1>404</h1>
      <p>Page not found</p>
      <a routerLink="/boards">Go to Boards</a>
    </div>
  `,
})
export class PageNotFoundComponent {}
