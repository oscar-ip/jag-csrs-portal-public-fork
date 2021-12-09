import { NgModule } from '@angular/core';
import { AuthModule, LogLevel } from 'angular-auth-oidc-client';


@NgModule({
    imports: [AuthModule.forRoot({
        config: {
            triggerAuthorizationResultEvent: true,
            postLoginRoute: '/welcomeuser',
            logLevel: LogLevel.Debug,
            historyCleanupOff: true,
            eagerLoadAuthWellKnownEndpoints: false,
            authority: 'https://dev.oidc.gov.bc.ca/auth/realms/onestopauth-basic',
            redirectUrl: 'https://jag-csrs-portal-public-f0b5b6-dev.apps.silver.devops.gov.bc.ca',
            postLogoutRedirectUri: 'https://jag-csrs-portal-public-f0b5b6-dev.apps.silver.devops.gov.bc.ca',
            clientId: 'jsb-csrs-991',
            scope: 'openid profile',
            //usePushedAuthorisationRequests: true, // use par Pushed Authorisation Requests
            responseType: 'code',
            silentRenew: true,
            silentRenewUrl: window.location.origin + '/silent-renew.html',
            useRefreshToken: true,
            renewTimeBeforeTokenExpiresInSeconds: 10,
            autoUserInfo: true,
            customParamsAuthRequest: {
              prompt: 'consent', // login, consent
            },


        }
      })],
    exports: [AuthModule],
})
export class AuthConfigModule {}
