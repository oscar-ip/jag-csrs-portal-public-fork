import {
  HttpClient,
  HttpClientModule,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { NgModule, APP_INITIALIZER, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
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
import { MailboxComponent } from './components/mailbox/mailbox.component';
import { CommunicationComponent } from './components/communication/communication.component';
import { MatIconModule } from '@angular/material/icon'
import { MatDialogModule, MAT_DIALOG_DATA, MAT_DIALOG_DEFAULT_OPTIONS } from '@angular/material/dialog';

export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

import { MatTabsModule } from '@angular/material/tabs';
import { ModalDialogComponent } from './components/modal-dialog/modal-dialog.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    LandingComponent,
    QuestionnaireComponent,
    WelcomeUserComponent,
    ApplicationFormStepperComponent,
    ChildApplicationQuestionComponent,
    MailboxComponent,
    CommunicationComponent,
    ModalDialogComponent,
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
    MatDialogModule,
    MatTabsModule,
    MatIconModule,
    MatDialogModule,
    FormsModule,
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
    {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: true}},
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
