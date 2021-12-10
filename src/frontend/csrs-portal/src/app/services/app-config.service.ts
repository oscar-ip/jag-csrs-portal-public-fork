import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
export interface IAppConfig {
  production: boolean;
  environment: string;
  version: string;
  bceIdLink: string;
  };
export class AppConfig implements IAppConfig {
  production: boolean;
  environment: string;
  version: string;
  bceIdLink: string;
}
@Injectable({
  providedIn: 'root',
})
export class AppConfigService {
  public appConfig: AppConfig;

  private BCE_ID_DEFAULT =
    'https://bceid.gov.bc.ca/' as const;

  constructor(private http: HttpClient) { }

  public loadAppConfig(): Observable<any> {
    return this.http.get('/assets/app.config.json').pipe(
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

  get bceIdtLink(): string {
    const link = this.appConfig?.bceIdLink;
    return link ? link : this.BCE_ID_DEFAULT;
  }
}
