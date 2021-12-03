export class AppRoutes {

  public static QUESTIONNAIRE = 'questionnaire';
  public static WELCOMEUSER = 'welcomeuser';
  public static STEPPERFORM = 'stepperform';
  public static APPLICATIONFORM = 'applicationform';
  public static routePath(route: string): string {
    return `/${route}`;
  }
}