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

  constructor(public oidcSecurityService : OidcSecurityService,
              @Inject(LoggerService) private logger,
              @Inject(Router) private router,
              @Inject(ActivatedRoute) private route,
              @Inject(AppConfigService) private appConfigService,
              @Inject(SnowplowService) private snowplow) {
  }

  public async ngOnInit() {

      this.cscLink = this.appConfigService.cscLink;

      //this.bceIdRegisterLink = environment.production ? 'https://www.bceid.ca/os/?7731' : 'https://www.development.bceid.ca/os/?2281';
      this.bceIdRegisterLink = 'https://www.test.bceid.ca/register/basic/account_details.aspx?type=regular&eServiceType=basic';

      this.oidcSecurityService.checkAuth().subscribe(({ isAuthenticated,
                                          userData,
                                          accessToken,
                                          idToken,
                                          configId,
                                          errorMessage }) => {
          this.logger.info('isAuthenticated: ', isAuthenticated);
          this.logger.info('userData: ', userData);
          this.logger.info('accessToken: ', accessToken);
          this.logger.info('idToken: ', idToken);
          this.logger.info('configId: ', configId);
          this.logger.info('errorMessage: ', errorMessage);

          if (isAuthenticated === true)
          {
            this.router.navigate(['/welcomeuser']);
          }
      });


    }

  login() {
    this.oidcSecurityService.authorize();
  }

  logout() {
    this.oidcSecurityService.logoff();
  }

  public ngAfterViewInit(): void {
    this.snowplow.refreshLinkClickTracking();
  }
}
