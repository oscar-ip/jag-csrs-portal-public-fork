import { Injectable } from '@angular/core';
import { LoggerService } from '@core/services/logger.service';
import { KeycloakService } from 'keycloak-angular';
import Keycloak, { KeycloakLoginOptions } from 'keycloak-js';
import { BehaviorSubject, from, Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

export interface IAuthService {
  login(options?: KeycloakLoginOptions): Promise<void>;
  loadUserProfile(options?: any): Promise<any>;
  isLoggedIn(): Promise<boolean>;
  logout(redirectUri: string): Promise<void>;
  getToken(): Promise<any>;
  getUserInfo(): Promise<any>;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
     private keycloakService: KeycloakService,
     private logger: LoggerService) {}

  public login(options?: any): Promise<any> {

    this.logger.info('inside keycloak login');
    const promiseVoid = this.keycloakService.login(options);
    this.logger.info(`after this.keycloakService.login(options)`);

    setTimeout(() => {
      if (this.keycloakService.getKeycloakInstance().onAuthSuccess)
      {
        console.log('authenticated');
      }
   });


    const loggedIn = this.keycloakService.isLoggedIn();
    this.logger.info(`loggedIn id ${loggedIn}`);
    if (loggedIn)
    {
      const token = this.keycloakService.getToken() as Promise<string> ;
      this.logger.info(`token is ${JSON.stringify(token)}`);
      localStorage.setItem('jwt', JSON.stringify(token));

      const userProfile = this.keycloakService.loadUserProfile() as Promise<Keycloak.KeycloakProfile>;
      this.logger.info(`userProfile is ${JSON.stringify(userProfile)}`);
      localStorage.setItem('user', JSON.stringify(userProfile));
    }

    this.logger.info(`before leaving this.keycloakService.login(options)`);
    return promiseVoid;
  }

  public loadUserProfile(options?: any): Promise<any> {
    return this.keycloakService.loadUserProfile();
  }


  public isLoggedIn(): Promise<boolean> {
    return this.keycloakService.isLoggedIn();
  }

  public logout(redirectUri: string = '/'): Promise<void> {
    return this.keycloakService.logout(redirectUri);
  }

  public async getToken(forceReload?: boolean): Promise<any> {
    const loggedIn = await this.keycloakService.isLoggedIn();
    this.logger.info('isLoggedIn', loggedIn);
    if (loggedIn) {
      return this.keycloakService.getToken() as Promise<any>;
    }
    return Promise.reject('token rejected.');
  }

  public async getUserInfo(forceReload?: boolean): Promise<any> {
    const loggedIn = await this.keycloakService.isLoggedIn();
    this.logger.info('isLoggedIn', loggedIn);
    if (loggedIn) {
      return this.keycloakService.loadUserProfile(forceReload) as Promise<any>;
    }
    return Promise.reject('user not logged in.');
  }
/*
  public async loadData () {
    document.getElementById('username').innerText = this.keycloakService.getKeycloakInstance().userInfo;

    var url = 'http://localhost:8080/restful-service';

    var req = new XMLHttpRequest();
    req.open('GET', url, true);
    req.setRequestHeader('Accept', 'application/json');
    req.setRequestHeader('Authorization', 'Bearer ' + this.keycloakService.getKeycloakInstance().token);

    req.onreadystatechange = function () {
        if (req.readyState == 4) {
            if (req.status == 200) {
                alert('Success');
            } else if (req.status == 403) {
                alert('Forbidden');
            }
        }
    }

    req.send();
};
*/

/*
this.keycloakService.getKeycloakInstance().updateToken(30).then(function() {
    loadData();
}).catch(function() {
    alert('Failed to refresh token');
});

this.keycloakService.getKeycloakInstance().updateToken(5)
    .then(function(refreshed) {
        if (refreshed) {
            alert('Token was successfully refreshed');
        } else {
            alert('Token is still valid');
        }
    }).catch(function() {
        alert('Failed to refresh the token, or the session has expired');
    });

*/
}

