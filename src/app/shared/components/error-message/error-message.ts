import { Component, input } from '@angular/core';

@Component({
  selector: 'app-error-message',
  template: `
    <div class="alert alert-danger my-3" role="alert">
      {{ message() || 'An unexpected error occurred.' }}
    </div>
  `,
})
export class ErrorMessageComponent {
  message = input<string>('');
}
