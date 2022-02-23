import { AfterViewInit, Component, OnInit, ViewEncapsulation  } from '@angular/core';
import { Inject } from '@angular/core';
import { LoggerService } from '@core/services/logger.service';

import { Router, ActivatedRoute } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { AppConfigService } from 'app/services/app-config.service';
import { SnowplowService } from '@core/services/snowplow.service';
import { environment } from './../../../environments/environment';

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
              @Inject(OidcSecurityService) private oidcSecurityService,
              @Inject(AppConfigService) private appConfigService,
              @Inject(SnowplowService) private snowplow) {

    const accessToken = oidcSecurityService.getAccessToken();

    if (accessToken) {
      this.logger.log('info', `accessToken: ${accessToken}`);
    }

    if (oidcSecurityService.isAuthenticated()) {
      this.logger.log('info', 'authenticated');
    } else {
      this.logger.log('info', 'not authenticated');
    }

  }

  public async ngOnInit() {

      this.cscLink = this.appConfigService.cscLink;

      this.bceIdRegisterLink = /*environment.production ? 'https://www.bceid.ca/os/?7731' :*/ 'https://www.development.bceid.ca/os/?2281';

      this.oidcSecurityService.checkAuth().subscribe(({ isAuthenticated, userData, accessToken, idToken }) => {

        this.logger.log('info',`isAuthenticated = ${isAuthenticated}`);
        this.logger.log('info',`userData = ${userData}`);
        this.logger.log('info',`accessToken = ${accessToken}`);
        this.logger.log('info',`idToken = ${idToken}`);

        if (isAuthenticated === true)
        {
          this.router.navigate(['/welcomeuser']);
        }
      });
    }

  login() {
    this.logger.log('info','inside login');
    this.oidcSecurityService.authorize();
  }

  logout() {
    this.logger.log('info','inside logout');
    this.oidcSecurityService.logoff();
  }

  public ngAfterViewInit(): void {
    this.snowplow.refreshLinkClickTracking();
  }
}
