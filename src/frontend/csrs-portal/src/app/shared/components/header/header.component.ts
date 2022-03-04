import {
  Component,
  ChangeDetectionStrategy,
  Output,
  EventEmitter,
  Input,
  OnInit
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


  constructor(protected logger: LoggerService,
              private appConfigService: AppConfigService,
              ) {

          this.toggle = new EventEmitter<void>();
  }

  public async ngOnInit() {
  }

  public toggleSidenav(): void {
    this.toggle.emit();
  }

}
