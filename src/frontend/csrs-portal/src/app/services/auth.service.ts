import { Injectable } from '@angular/core';
import { LoggerService } from '@core/services/logger.service';
import { User } from '@shared/models/user.model';
import { BehaviorSubject, from, Observable } from 'rxjs';
import { take } from 'rxjs/operators';

export interface IAuthService {
  isLoggedIn(): Promise<boolean>;
  logout(redirectUri: string): Promise<void>;
  getUser(forceReload?: boolean): Promise<User>;
  getUser$(forceReload?: boolean): Observable<User>;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private logger: LoggerService
  ) {}

  public login(options?: any): Promise<void> {
    return options;
  }
}
