import { Component, OnInit } from '@angular/core';
import { AccountService } from 'app/api/api/account.service';
import { LoggerService } from '@core/services/logger.service';
import { Inject} from '@angular/core';
import { HttpClient, HttpStatusCode, HttpResponse } from '@angular/common/http';
import { AccountFileSummary } from 'app/api/model/accountFileSummary.model';
import { Router } from '@angular/router';
import { AppRoutes } from 'app/app.routes';
@Component({
  selector: 'app-welcome-user',
  templateUrl: './welcome-user.component.html',
  styleUrls: ['./welcome-user.component.scss']
})
export class WelcomeUserComponent implements OnInit {
  _reponse: HttpResponse<AccountFileSummary>;

  constructor(@Inject(AccountService) private accountService,
              @Inject(LoggerService) private logger,
              @Inject(Router) private router) {}

  ngOnInit(): void {

    this.accountService.apiAccountGet('response', false).subscribe({
      next: (data) => {
        this._reponse = data;
        if (this._reponse.status === HttpStatusCode.NotFound) {
          //redirect to application form
          this.router.navigate(AppRoutes.routePath(AppRoutes.APPLICATIONFORM));
        }
        else
          if (this._reponse.status === HttpStatusCode.Ok) {
            if (this._reponse.body.files === null || this._reponse.body.files.length === 0)
            {
              //redirect to application form
              this.router.navigate(AppRoutes.routePath(AppRoutes.APPLICATIONFORM));
            }
          }
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
