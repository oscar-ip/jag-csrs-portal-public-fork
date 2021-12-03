import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ChildApplicationQuestionComponent } from '@components/child-application-question/child-application-question.component';
import { WelcomeUserComponent } from '@components/welcome-user/welcome-user.component';
import { AppRoutes } from './app.routes';
import { ApplicationFormStepperComponent } from './components/application-form-stepper/application-form-stepper.component';
import { LandingComponent } from './components/landing/landing.component';
import { QuestionnaireComponent } from './components/questionnaire/questionnaire.component';

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
  },
  {
    path: AppRoutes.STEPPERFORM,
    component: ApplicationFormStepperComponent,
  },
  {
    path: AppRoutes.APPLICATIONFORM,
    component: ChildApplicationQuestionComponent,
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
