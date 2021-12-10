import { Component, OnInit, ViewEncapsulation  } from '@angular/core';
import { Config } from '@config/config.model';
import { AuthService } from '@core/services/auth.service';
import { Inject } from '@angular/core';
import { LoggerService } from '@core/services/logger.service';

import { Router, ActivatedRoute } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LandingComponent implements OnInit {

  public isLoggedIn = false;
  public bceIdLink: string;
  public bceIdRegisterLink: string;
  public cscLink: string;
  public welcomeUser: string;
  public code: string;

  constructor(@Inject(LoggerService) private logger,
              @Inject(Router) private router,
              @Inject(ActivatedRoute) private route,
              @Inject(OidcSecurityService) private oidcSecurityService) {
    this.bceIdLink = 'https://www.bceid.ca/';
    this.cscLink = 'https://www.childsupportcalculator.ca/british-columbia.html';
    this.bceIdRegisterLink = 'https://www.bceid.ca/register/basic/account_details.aspx?type=regular&eServiceType=basic';
    console.log(`constructor`);

    const accessToken = oidcSecurityService.getAccessToken();

    if (accessToken) {
      console.log('accessToken: ' + accessToken);
    }

    if (oidcSecurityService.isAuthenticated()) {
      console.log('authenticated');
    } else {
      console.log('not authenticated');
    }

  }

  public async ngOnInit() {

      this.oidcSecurityService.checkAuth().subscribe(({ isAuthenticated, userData, accessToken, idToken }) => {

        console.log(`isAuthenticated = ${isAuthenticated}`);
        console.log(`userData = ${userData}`);
        console.log(`accessToken = ${accessToken}`);
        console.log(`idToken = ${idToken}`);

        console.dir({ isAuthenticated, userData, accessToken, idToken });
        if (isAuthenticated === true)
        {
          this.router.navigate(['/welcomeuser']);
        }
      });
    }

  login() {
    console.log('inside login');
    this.oidcSecurityService.authorize();
  }

  logout() {
    console.log('inside logout');
    this.oidcSecurityService.logoff();
  }
}
