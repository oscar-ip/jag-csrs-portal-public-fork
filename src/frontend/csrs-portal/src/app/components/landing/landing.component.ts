import { Component, OnInit, ViewEncapsulation  } from '@angular/core';
import { Config } from '@config/config.model';
import { AuthService } from '@core/services/auth.service';
import { Inject } from '@angular/core';
import { LoggerService } from '@core/services/logger.service';

import { KeycloakService } from 'keycloak-angular';
import { KeycloakProfile, KeycloakLoginOptions } from 'keycloak-js';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LandingComponent implements OnInit {

  public isLoggedIn = false;
  public userProfile: KeycloakProfile | null = null;
  public options: KeycloakLoginOptions | null = null;
  public bceIdLink: string;
  public bceIdRegisterLink: string;
  public cscLink: string;
  public welcomeUser: string;
  public code: string;

  constructor(@Inject(AuthService) private authService,
              @Inject(LoggerService) private logger,
              @Inject(KeycloakService) private  keycloakService,
              @Inject(Router) private router,
              @Inject(ActivatedRoute) private route) {
    this.bceIdLink = 'https://www.bceid.ca/';
    this.cscLink = 'https://www.childsupportcalculator.ca/british-columbia.html';
    this.bceIdRegisterLink = 'https://www.bceid.ca/register/basic/account_details.aspx?type=regular&eServiceType=basic';
    console.log(`constructor`);
  }

  public async ngOnInit() {
    this.isLoggedIn = await this.keycloakService.isLoggedIn();
    console.log(`isLoggedIn is ${this.isLoggedIn}`);

    if (this.isLoggedIn) {
      this.userProfile = await this.keycloakService.loadUserProfile();
      console.log(`userProfile is ${this.userProfile}`);

      this.route.queryParams
      .filter(params => params.code)
      .subscribe(params => {
        console.log(params);
        this.code = params.code;
        console.log(this.code);
      });

      if (this.code)
      {
        // here should be called service to transfer code thru api to backend ....
      }


      this.router.navigate(['/welcomeuser']);
    }
  }

  public login(): void {
      console.log('login');



      this.authService.login({
        redirectUri: 'https://jag-csrs-portal-public-f0b5b6-dev.apps.silver.devops.gov.bc.ca'//,
        //onLoad: 'login-required',
        //flow: 'hybrid',
        //enableLogging: true
      });
  }

  public logout() {
    console.log('logout');
    this.authService.logout();
  }

}
