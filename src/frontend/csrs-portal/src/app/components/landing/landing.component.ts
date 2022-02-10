import { AfterViewInit, Component, OnInit, ViewEncapsulation  } from '@angular/core';
import { Inject } from '@angular/core';
import { LoggerService } from '@core/services/logger.service';

import { Router, ActivatedRoute } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { AppConfigService } from 'app/services/app-config.service';
import { SnowplowService } from '@core/services/snowplow.service';

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
  public _logger: LoggerService;
  public _config: AppConfigService;
  public _snowplow: SnowplowService;

  constructor(@Inject(LoggerService) private logger,
              @Inject(Router) private router,
              @Inject(ActivatedRoute) private route,
              @Inject(OidcSecurityService) private oidcSecurityService,
              @Inject(AppConfigService) private appConfigService,
              @Inject(SnowplowService) private snowplow) {

    this._config = appConfigService;
    this._logger = logger;
    this._logger.log('info', 'logger:constructor');
    this._snowplow = snowplow;

    const accessToken = oidcSecurityService.getAccessToken();

    if (accessToken) {
      this._logger.log('info', `accessToken: ${accessToken}`);
    }

    if (oidcSecurityService.isAuthenticated()) {
      this._logger.log('info', 'authenticated');
    } else {
      this._logger.log('info', 'not authenticated');
    }

  }

  public async ngOnInit() {

      this.cscLink = this._config.cscLink;
      this.bceIdRegisterLink = this._config.bceIdRegisterLink;

      this.oidcSecurityService.checkAuth().subscribe(({ isAuthenticated, userData, accessToken, idToken }) => {

        this._logger.log('info',`isAuthenticated = ${isAuthenticated}`);
        this._logger.log('info',`userData = ${userData}`);
        this._logger.log('info',`accessToken = ${accessToken}`);
        this._logger.log('info',`idToken = ${idToken}`);

        if (isAuthenticated === true)
        {
          this.router.navigate(['/welcomeuser']);
        }
      });
    }

  login() {
    this._logger.log('info','inside login');
    this.oidcSecurityService.authorize();
  }

  logout() {
    this._logger.log('info','inside logout');
    this.oidcSecurityService.logoff();
  }

  public ngAfterViewInit(): void {
    this.snowplow.refreshLinkClickTracking();
  }
}
