import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideAuth0 } from '@auth0/auth0-angular';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(),
    provideAuth0({
      domain: 'eleon88.us.auth0.com',
      clientId: 'uGhqQcfAwb1qock4381gaeL2nwu2fP6X',
      authorizationParams: {
        redirect_uri: window.location.origin,
      },
    }),
  ],
};
