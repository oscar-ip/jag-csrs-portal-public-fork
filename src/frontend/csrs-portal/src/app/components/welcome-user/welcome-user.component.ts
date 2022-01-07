import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { JsonPipe } from '@angular/common';
import { AccountService } from 'app/api/api/account.service';
import { LookupService } from 'app/api/api/lookup.service';
import { LoggerService } from '@core/services/logger.service';
import { of } from 'rxjs';
import { Inject, Injectable, Optional }                      from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpStatusCode,
         HttpResponse, HttpEvent, HttpParameterCodec }       from '@angular/common/http';
import { Observable }                                        from 'rxjs';
import { AccountFileSummary } from 'app/api/model/accountFileSummary.model';
import { Router, ActivatedRoute } from '@angular/router';
import { AppRoutes } from 'app/app.routes';


@Component({
  selector: 'app-welcome-user',
  templateUrl: './welcome-user.component.html',
  styleUrls: ['./welcome-user.component.scss']
})
export class WelcomeUserComponent implements OnInit {
  _logger: LoggerService;
  _reponse: HttpResponse<AccountFileSummary>;

  constructor(@Inject(AccountService) private accountService,
              @Inject(LoggerService) private logger,
              @Inject(Router) private router) {}

  ngOnInit(): void {
    this._logger = this.logger;


    this.accountService.apiAccountGet('response', false).subscribe({
      next: (data) => {
        this._reponse = data;
        if (this._reponse.status === HttpStatusCode.NotFound) {
          //redirect to empty application form
          this.router.navigate(AppRoutes.routePath(AppRoutes.APPLICATIONFORM));
        }
        if (this._reponse.status === HttpStatusCode.Ok) {
          if (this._reponse.body.files === null || this._reponse.body.files.length === 0)
          {
            //redirect to empty application from
            this.router.navigate(AppRoutes.routePath(AppRoutes.APPLICATIONFORM));
          }
          else
          {
            if (this._reponse.body.files.length > 0)
            {
              if (this._reponse.body.files[0].status === 'Draft')
              {
                //redirect to Application form with draft application
                this.router.navigate(AppRoutes.routePath(AppRoutes.APPLICATIONFORM));
                //how application form populates by draft application ?
              }
              else
                if (this._reponse.body.files[0].status === 'Active') {}
            }
          }
        }
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this._logger.error(e.error.message);
        } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this._logger.info('Backend returned ', e);
          }
      },
      complete: () => this._logger.info('apiAccountGet<AccountFileSummary> is completed')
    })




  }

}
