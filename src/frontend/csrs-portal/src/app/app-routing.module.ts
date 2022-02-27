import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ChildApplicationQuestionComponent } from '@components/child-application-question/child-application-question.component';
import { MailboxComponent } from '@components/mailbox/mailbox.component';
import { WelcomeUserComponent } from '@components/welcome-user/welcome-user.component';
import { AppRoutes } from './app.routes';
import { ApplicationFormStepperComponent } from './components/application-form-stepper/application-form-stepper.component';
import { LandingComponent } from './components/landing/landing.component';
import { QuestionnaireComponent } from './components/questionnaire/questionnaire.component';
import { CommunicationComponent } from './components/communication/communication.component';
import { AutoLoginPartialRoutesGuard } from 'angular-auth-oidc-client';

const routes: Routes = [

  {
    path: '',
    component: LandingComponent,
  },
  {
    path: AppRoutes.QUESTIONNAIRE,
    component: QuestionnaireComponent,
  },
  {
    path: AppRoutes.WELCOMEUSER,
    component: WelcomeUserComponent,
    canActivate: [AutoLoginPartialRoutesGuard],
  },
  {
    path: AppRoutes.STEPPERFORM,
    component: ApplicationFormStepperComponent,
    canActivate: [AutoLoginPartialRoutesGuard],
  },
  {
    path: AppRoutes.APPLICATIONFORM,
    component: ChildApplicationQuestionComponent,
    canActivate: [AutoLoginPartialRoutesGuard],
  },
  {
    path: AppRoutes.MAILBOX,
    component: MailboxComponent,
  },
  {
    path: AppRoutes.COMMUNICATIONFORM,
    component: CommunicationComponent,
    canActivate: [AutoLoginPartialRoutesGuard],
  },
  {
    path: '**',
    redirectTo: '/',
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
