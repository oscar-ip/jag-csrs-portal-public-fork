import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LogInOutService {

  private logoutStatus:BehaviorSubject<string> = new BehaviorSubject<string>('');
  private currentStatus:BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private currentPortalUser: BehaviorSubject<string> = new BehaviorSubject<string>('');

  getLogoutStatus = this.logoutStatus.asObservable();
  getCurrentStatus = this.currentStatus.asObservable();
  getCurrentPortalUser = this.currentPortalUser.asObservable();

  constructor() { }

  logoutUser(status){
    this.logoutStatus.next(status);
  }

  currentUser(status){
    this.currentStatus.next(status);
  }

  emitCurrentPortalUser(user) {
    this.currentPortalUser.next(user);
  }


}
