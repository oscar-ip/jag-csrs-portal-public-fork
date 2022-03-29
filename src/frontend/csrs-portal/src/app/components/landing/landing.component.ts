import { AfterViewInit, Component, OnInit, ViewEncapsulation, isDevMode } from '@angular/core';
import { Inject } from '@angular/core';
import { LoggerService } from '@core/services/logger.service';

import { Router, ActivatedRoute } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { AppConfigService } from 'app/services/app-config.service';
import { SnowplowService } from '@core/services/snowplow.service';
import { environment } from './../../../environments/environment';
import { LogInOutService } from 'app/services/log-in-out.service';
import { questionnaireClickData } from '@components/model/snowplowData.model';
@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LandingComponent implements OnInit
{

  public isLoggedIn = false;
  public bceIdLink: string;
  public bceIdRegisterLink: string;
  public cscLink: string;
  public welcomeUser: string;
  public code: string;
  public questionClickData: questionnaireClickData;

  constructor(public oidcSecurityService : OidcSecurityService,
              private logInOutService : LogInOutService,
              @Inject(LoggerService) private logger,
              @Inject(Router) private router,
              @Inject(ActivatedRoute) private route,
              @Inject(AppConfigService) private appConfigService,
              @Inject(SnowplowService) private snowplow) {
  }

  public async ngOnInit() {

      this.cscLink = this.appConfigService.appConfig.cscLink;
      this.bceIdRegisterLink = this.appConfigService.appConfig.bceIdRegisterLink;

      this.logInOutService.getLogoutStatus.subscribe((data) => {
        if (data !== null || data !== '')
        {
          if(data === 'BCeID Login'){
            this.login();
          }
          else
            if(data === 'Logout'){
              this.logout();
            }
        }
      })

      this.oidcSecurityService.checkAuth().subscribe(
        ({ isAuthenticated}) => {
          if (isAuthenticated === true)
          {
            this.router.navigate(['/welcomeuser']);
          }
          else
          {
            this.router.navigate(['/']);
          }

          this.logInOutService.currentUser(isAuthenticated);
      });
  }

  login() {
    this.questionClickData = {
      step: 0,
      question: 'Is the recalculation service right for you?',
      label: 'BCeID Login',
      url: window.location.href
    };
    this.snowplow.trackSelfDescribingEventClick(this.questionClickData);

    this.oidcSecurityService.authorize();
  }

  here() {
    this.questionClickData = {
      step: 0,
      question: 'Is the recalculation service right for you?',
      label: 'If you already have a Basic BCeID account log in here.',
      url: window.location.href
    };
    this.snowplow.trackSelfDescribingEventClick(this.questionClickData);

    this.oidcSecurityService.authorize();
  }

  logout() {

    this.oidcSecurityService.logoffAndRevokeTokens();
  }


  register() {
    this.questionClickData = {
      step: 0,
      question: "Is the recalculation service right for you?",
      label:    "Register for a Basic BCeID",
      url: window.location.href
     };
     this.snowplow.trackSelfDescribingEventClick(this.questionClickData);
  }

  learnMore() {
    this.questionClickData = {
      step: 0,
      question: "Is the recalculation service right for you?",
      label:    "Learn More >",
      url: window.location.href
     };
     this.snowplow.trackSelfDescribingEventClick(this.questionClickData);
  }


  questionnaire() {
    this.questionClickData = {
      step: 0,
      question: "Is the recalculation service right for you?",
      label:    "Questionnaire",
      url: window.location.href
     };
     this.snowplow.trackSelfDescribingEventClick(this.questionClickData);
  }

  startQuestionnaire() {
    this.questionClickData = {
      step: 0,
      question: "Is the recalculation service right for you?",
      label:    "Start Questionnaire",
      url: window.location.href
     };
     this.snowplow.trackSelfDescribingEventClick(this.questionClickData);
  }

  public ngAfterViewInit(): void {
    this.snowplow.refreshLinkClickTracking();
  }

}
