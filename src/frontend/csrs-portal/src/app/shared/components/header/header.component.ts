import {
  Component,
  ChangeDetectionStrategy,
  OnInit
} from '@angular/core';
import { LoggerService } from '@core/services/logger.service';
import { AppConfigService } from 'app/services/app-config.service';
import { LogInOutService } from 'app/services/log-in-out.service';
import { OidcSecurityService } from 'angular-auth-oidc-client';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class HeaderComponent implements OnInit {
  public btnLabel: string = '';
  public btnIcon: string = '';
  isMobile: boolean = false;

  constructor(protected logger: LoggerService,
              private appConfigService: AppConfigService,
              private logInOutService: LogInOutService,
              public oidcSecurityService: OidcSecurityService) {
  }

  public async ngOnInit() {

    this.btnLabel = 'BCeID Login / Logout';
    this.btnIcon = 'login';

    if (window.innerWidth < 442) {
      this.isMobile = true;
    } else {
      this.isMobile = false;
    }

  }

  public onClickBtn()
  {
    //this.logInOutService.logoutUser(this.btnLabel);
    if (!this.oidcSecurityService.isAuthenticated())
    {
      this.oidcSecurityService.authorize();
    }
    else
    {
      this.oidcSecurityService.logoff();
      this.oidcSecurityService.revokeAccessToken(this.oidcSecurityService.getAccessToken);
    }
  }
}
