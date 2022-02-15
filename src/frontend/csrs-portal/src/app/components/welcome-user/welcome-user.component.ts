import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
  FormArray,
} from '@angular/forms';
import { AccountService } from 'app/api/api/account.service';
import { LoggerService } from '@core/services/logger.service';
import { Inject} from '@angular/core';
import { HttpClient, HttpStatusCode, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppRoutes } from 'app/app.routes';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '@shared/dialogs/confirm-dialog/confirm-dialog.component';
import { List, Dictionary } from 'ts-generic-collections-linq';
import { ModalDialogComponent } from 'app/components/modal-dialog/modal-dialog.component';
import { DialogOptions } from '@shared/dialogs/dialog-options.model';


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

  accountFormGroup: FormGroup;

  _reponse: HttpResponse<AccountFileSummary>;

  //csrsPartyFileIds: CSRSPartyFileIds = null;
  csrsAccount: CSRSAccount = null;
  data: any = null;

  constructor(private _formBuilder: FormBuilder, private http: HttpClient,
              @Inject(AccountService) private accountService,
              @Inject(LoggerService) private logger,
              @Inject(Router) private router,
              @Inject(OidcSecurityService) private oidc,
              public dialog: MatDialog) {}

  ngOnInit(): void {

    this.accountFormGroup = this._formBuilder.group({
      fileNumber: ['', Validators.required],
      password: ['', Validators.required]
    });

    //this.checkAccount();

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
      complete: () => {
        this.logger.info('apiAccountGet<AccountFileSummary> is completed');
        this.router.navigate(AppRoutes.routePath(AppRoutes.STEPPERFORM));
      }
    })
  }

  openModalDialog(): void {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '450px',
      data: this.data,
    });

    dialogRef.afterClosed().subscribe(result => {
      this.logger.info(`Dialog result: ${result}`);
    });
  }

  checkAccount(){

    this.data = {
      type: 'error',
      title: 'Technical error',
      content: 'Your account setup request could not be submitted.',
      content_normal: 'Please try again, or contact the Recalculation Service toll free',
      content_link: '1-866-660-2644',
      weight: 'bold',
      color: 'red'
    };

    const accountData = this.accountFormGroup.value;
    const csrsAccount: CSRSAccount = {fileNumber: accountData.fileNumber, referenceNumber: accountData.password };

    this.accountService.configuration.accessToken =  this.oidc.getAccessToken();
    this.accountService.apiAccountCheckcsrsaccountPost(csrsAccount).subscribe({
      next: (outData:any) => {
        var partyId = outData.partyId;
        var fileId = outData.fileId;
        //this.logger.info("account.partyId", partyId);
        //this.logger.info("account.fileId", fileId);

        if (!partyId || !fileId)
        {
          this.openModalDialog();
        }
        else
          if (partyId && fileId)
          {
            this.router.navigate(['/stepperform'], { queryParams: { partyId: partyId, fileId: fileId} });
          }

      },
      error: (e) => {
        this.openModalDialog();

        if (e.error instanceof Error) {
          this.logger.error(e.error.message);
        } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this.logger.info('Backend returned ', e);
        }
      },
      complete: () => this.logger.info('apiAccountCheckcsrsaccountPost is completed')
    })
  }




}
