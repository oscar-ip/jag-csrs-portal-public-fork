import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
export interface IAppConfig {
  production: boolean;
  environment: string;
  version: string;
  cscLink: string;
  bceIdRegisterLink: string;
  };
export class AppConfig implements IAppConfig {
  production: boolean;
  environment: string;
  version: string;
  cscLink: string;
  bceIdRegisterLink: string;
  bceIdRegisterLink_P: string;
  downloadApplication: string;
}
@Injectable({
  providedIn: 'root',
})
export class AppConfigService {
  public appConfig: AppConfig;

  constructor(private http: HttpClient) {
      this.loadAppConfig();
  }

  public loadAppConfig(): Observable<any> {
    return this.http.get('/assets/config/app.config.json').pipe(
      map((config: AppConfig) => {
        this.appConfig = config;
      })
    );
  }

  get production(): boolean {
    return this.appConfig?.production;
  }

  get environment(): string {
    return this.appConfig?.environment;
  }

  get version(): string {
    return this.appConfig?.version;
  }

  get cscLink(): string {
    return this.appConfig?.cscLink;
  }

  public get bceIdRegisterLink(): string {
    return this.appConfig?.bceIdRegisterLink;
  }

  public get bceIdRegisterLink_P(): string {
    return this.appConfig?.bceIdRegisterLink_P;
  }

  public get downloadApplication(): string {
    return this.appConfig?.downloadApplication;
  }

}
