import {
  Component,
  ChangeDetectionStrategy,
  Output,
  EventEmitter,
  Input,
  OnInit,
  Inject,
} from '@angular/core';
import { LoggerService } from '@core/services/logger.service';
import { TranslateService } from '@ngx-translate/core';
import { AppConfigService } from 'app/services/app-config.service';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Router } from '@angular/router';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class HeaderComponent implements OnInit {
  public fullName: string;
  @Output() public toggle: EventEmitter<void>;

  public icon: string = '';
  public iconText:string = '';

  constructor(public oidc : OidcSecurityService,
              protected logger: LoggerService,
              private appConfigService: AppConfigService,
              @Inject(Router) private router) {

          this.toggle = new EventEmitter<void>();
  }

  public async ngOnInit() {

    this.icon = 'login';
    this.iconText = 'BCeID Login';
    this.oidc.checkAuth().subscribe(({ isAuthenticated }) => {
      this.logger.info('isAuthenticated: ', isAuthenticated);
      if (isAuthenticated === true)
      {
        this.router.navigate(['/welcomeuser']);
      }
      else
      {
        this.router.navigate(['/']);
      }

    });

  }

  public toggleSidenav(): void {
    this.toggle.emit();
  }

  public changeIcon(): void{
    this.logger.warn(`inside: icon: ${this.icon} text: ${this.iconText}`);

    if (this.icon === 'login')
    {
      this.icon = 'logout';
      this.iconText = 'BCeID Logout';
      this.login();
    }
    else
    {
      this.icon = 'login';
      this.iconText = 'BCeID Login';
      this.logout();
    }
    this.logger.warn(`return: icon: ${this.icon} text: ${this.iconText}`);
  }

  login() {
    this.oidc.authorize();
  }

  logout() {
    this.oidc.logoff();
  }

}
