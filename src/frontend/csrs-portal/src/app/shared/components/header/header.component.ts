import {
  Component,
  ChangeDetectionStrategy,
  Output,
  EventEmitter,
  Input,
  OnInit,
} from '@angular/core';
import { LoggerService } from '@core/services/logger.service';
import { TranslateService } from '@ngx-translate/core';
import { AppConfigService } from 'app/services/app-config.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class HeaderComponent implements OnInit {
  public fullName: string;
  @Output() public toggle: EventEmitter<void>;

  public environment: string;
  public version: string;

  constructor(
    // protected authService: AuthService,
    protected logger: LoggerService,
    private appConfigService: AppConfigService,
    private translateService: TranslateService
  ) {
    this.toggle = new EventEmitter<void>();
 
    this.environment = this.appConfigService.environment;
    this.version = this.appConfigService.version;
  }

  public async ngOnInit() {
  }

  public toggleSidenav(): void {
    this.toggle.emit();
  }

}
