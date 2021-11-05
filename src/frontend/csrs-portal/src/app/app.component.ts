import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, RouterEvent } from '@angular/router';
import { Meta, Title } from '@angular/platform-browser';
import { ConfigService } from '@config/config.service';
import { TranslateService } from '@ngx-translate/core';
import { UtilsService } from '@core/services/utils.service';
import { RouteStateService } from '@core/services/route-state.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  constructor(
    private routeStateService: RouteStateService,
    private translateService: TranslateService,
    private titleService: Title,
    private configService: ConfigService,
    private metaTagService: Meta,
    private router: Router,
    private utilsService: UtilsService
  ) {
    this.router.events.subscribe((evt) => {
      if (evt instanceof NavigationEnd) {
        
      }
    });
  }

  public ngOnInit(): void {
    const onNavEnd = this.routeStateService.onNavigationEnd();
    this.scrollTop(onNavEnd);

    this.metaTagService.addTags([
      {
        name: '',
        content: '',
      },
      {
        name: 'description',
        content:
          '',
      },
      {
        name: 'keywords',
        content:
          '',
      },
    ]);

    this.translateService
      .get([
        'app_heading',
        
      ])
      .subscribe((translations) => {
        this.titleService.setTitle(
          this.translateService.instant('app_heading')
        );
      });
  }

  /**
   * @description
   * Scroll the page to the top on route event.
   */
  private scrollTop(routeEvent: Observable<RouterEvent>) {
    routeEvent.subscribe(() => this.utilsService.scrollTop());
  }
}
