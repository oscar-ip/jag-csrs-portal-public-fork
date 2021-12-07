import { NgModule, APP_INITIALIZER, Injector } from '@angular/core';
import { Router } from '@angular/router';

import { KeycloakAngularModule, KeycloakService } from 'keycloak-angular';

import { ToastService } from '@core/services/toast.service';
import { AuthService } from '@core/services/auth.service';
import { Config } from '../../config/config.model';

function initializer(
  keycloak: KeycloakService,
  injector: Injector,
): () => Promise<void> {
  const routeToDefault = () => injector.get(Router).navigateByUrl('/');

  return async (): Promise<void> => {
    const authenticated = await keycloak.init( {
         config: {
              url: 'https://dev.oidc.gov.bc.ca/auth',
              realm: 'onestopauth-basic',
              clientId: 'jsb-csrs-991'
          },
          initOptions: {
            //onLoad: 'login-required',
            //flow: 'hybrid',
            //enableLogging: true,
            //silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html',
         }
        }
      );


    keycloak.getKeycloakInstance().onTokenExpired = () => {
      keycloak.updateToken().catch(() => {
        injector
          .get(ToastService)
          .openErrorToast(
            'Your session has expired, you will need to re-authenticate'
          );
        routeToDefault();
      });
    };

    if (authenticated) {
      // Ensure configuration is populated before the application
      // is fully initialized to prevent race conditions

      // Force refresh to begin expiry timer.
      keycloak.updateToken(-1);
    }
  };
}

@NgModule({
  imports: [KeycloakAngularModule],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initializer,
      multi: true,
      deps: [KeycloakService, Injector],
    },
    AuthService,
  ],
})
export class KeycloakModule {}

