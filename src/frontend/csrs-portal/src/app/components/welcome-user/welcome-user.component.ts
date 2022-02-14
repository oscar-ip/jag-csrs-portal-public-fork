import { Component, OnInit } from '@angular/core';
import { AccountService } from 'app/api/api/account.service';
import { LoggerService } from '@core/services/logger.service';
import { Inject} from '@angular/core';
import { HttpClient, HttpStatusCode, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppRoutes } from 'app/app.routes';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { List, Dictionary } from 'ts-generic-collections-linq';

// -- import data structure
import {
  AccountFileSummary,
  Party,
  FileSummary,
  CSRSAccount,
  CSRSPartyFileIds
} from 'app/api/model/models';

@Component({
  selector: 'app-welcome-user',
  templateUrl: './welcome-user.component.html',
  styleUrls: ['./welcome-user.component.scss']
})
export class WelcomeUserComponent implements OnInit {
  _reponse: HttpResponse<AccountFileSummary>;

  constructor(@Inject(AccountService) private accountService,
              @Inject(LoggerService) private logger,
              @Inject(Router) private router,
              @Inject(OidcSecurityService) private oidc) {}

  ngOnInit(): void {
    this.accountService.configuration.accessToken =  this.oidc.getAccessToken();
    this.accountService.apiAccountGet('response', false).subscribe({
      next: (data:any) => {
        this._reponse = data;
        this.logger.info('data',data);

        var user = data.body.user;
        var files = data.body.files;

        this.logger.info("user", user);
        this.logger.info("files", files);
        this.logger.info("data.status", data.status);




        if (data.status === HttpStatusCode.NotFound ||
            ( data.status === HttpStatusCode.Ok && ( files === null || files.length === 0))) {
              //redirect to application form
              this.logger.info("redirect to AppRoutes.APPLICATIONFORM");
              //this.router.navigate(AppRoutes.routePath(AppRoutes.APPLICATIONFORM));
            }

        if ( data.status === HttpStatusCode.Ok && ( files != null && files.length > 0)) {

            let listFiles = new List<FileSummary>(files);
            let activeStatus:FileSummary = listFiles.firstOrDefault(x=>x.status == 'Active');
            if (activeStatus!= null)
            {
              this.logger.info("redirect to AppRoutes.COMMUNICATIONFORM");
              //this.router.navigate(AppRoutes.routePath(AppRoutes.COMMUNICATIONFORM));
            }
          }


        /*
        if (this._reponse.status === HttpStatusCode.NotFound ||
            ( this._reponse.status === HttpStatusCode.Ok &&
              (this._reponse.body.files === null || this._reponse.body.files.length === 0))) {
          //redirect to application form
          this.router.navigate(AppRoutes.routePath(AppRoutes.APPLICATIONFORM));
        }*/

      },
      error: (e) => {
        if (e.error instanceof Error) {
          this.logger.error(e.error.message);
        } else {
            //Backend returns unsuccessful response codes such as: 500 etc.
            this.logger.info('Backend returned ', e);
          }
      },
      complete: () => this.logger.info('apiAccountGet<AccountFileSummary> is completed')
    })
  }
}
