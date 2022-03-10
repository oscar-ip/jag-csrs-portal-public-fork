import {
  Component,
  ChangeDetectionStrategy,
  OnInit
} from '@angular/core';
import { LoggerService } from '@core/services/logger.service';
import { AppConfigService } from 'app/services/app-config.service';
import { LogInOutService } from 'app/services/log-in-out.service';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class HeaderComponent implements OnInit {
  public btnLabel: string = '';
  public btnIcon: string = '';

  constructor(protected logger: LoggerService,
              private appConfigService: AppConfigService,
              private logInOutService: LogInOutService,
              ) {
  }

  public async ngOnInit() {

    this.logInOutService.getCurrentStatus.subscribe((data) => {
      if (data !== null || data !== undefined)
      {
        if(data === true){

          this.btnLabel = 'Logout';
          this.btnIcon = 'logout';
        }
        else
        {
          this.btnLabel = 'BCeID Login';
          this.btnIcon = 'login';
        }
      }
    })
  }

  public onClickBtn()
  {
    this.logInOutService.logoutUser(this.btnLabel);
    if (this.btnLabel === 'BCeID Login')
    {
      this.btnLabel = 'Logout';
      this.btnIcon = 'logout';
    }
    else
    {
      this.btnLabel = 'BCeID Login';
      this.btnIcon = 'login';
      //this.router.navigate(['/']);
    }
  }
}
