import {
  HttpClient,
  HttpClientModule,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgBusyModule } from 'ng-busy';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConfigModule } from './config/config.module';
import { CoreModule } from './core/core.module';
import { NgxMaterialModule } from './shared/modules/ngx-material/ngx-material.module';
import { SharedModule } from './shared/shared.module';
import {
  TranslateModule,
  TranslateLoader,
  TranslateService,
} from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { LandingComponent } from './components/landing/landing.component';
import { MatStepperModule } from '@angular/material/stepper';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { AppConfigService, AppConfig } from './services/app-config.service';

import { WindowRefService } from '@core/services/window-ref.service';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { CdkAccordionModule} from '@angular/cdk/accordion';
import { MatGridListModule } from '@angular/material/grid-list';
import { QuestionnaireComponent } from './components/questionnaire/questionnaire.component';
import { WelcomeUserComponent } from './components/welcome-user/welcome-user.component';
import { ApplicationFormStepperComponent } from './components/application-form-stepper/application-form-stepper.component';
import { ChildApplicationQuestionComponent } from './components/child-application-question/child-application-question.component';

import { AuthConfigModule } from './auth/auth-config.module';
import { ApiModule } from './api/api.module';
import { Configuration } from './api/configuration';
import { OidcSecurityService } from 'angular-auth-oidc-client';

export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
@NgModule({
  declarations: [
    AppComponent,
    LandingComponent,
    QuestionnaireComponent,
    WelcomeUserComponent,
    ApplicationFormStepperComponent,
    ChildApplicationQuestionComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    CoreModule,
    SharedModule,
    ConfigModule,
    HttpClientModule,
    MatStepperModule,
    CdkAccordionModule,
    MatGridListModule,
    AuthConfigModule,
    ApiModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
      isolate: false,
      extend: true,
    }),
  ],
  exports: [NgBusyModule, NgxMaterialModule, TranslateModule],
  providers: [
    CurrencyPipe,
    AppConfigService,
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { showError: true },
      useFactory: (authService: OidcSecurityService) => new Configuration (
        {
          basePath: '',//environment.apiUrl,
          accessToken: authService.getAccessToken.bind(authService)
        }
      ),
      deps: [OidcSecurityService],
      multi: false
    },
    WindowRefService,
  ],
  bootstrap: [AppComponent],
})



export class AppModule {}
