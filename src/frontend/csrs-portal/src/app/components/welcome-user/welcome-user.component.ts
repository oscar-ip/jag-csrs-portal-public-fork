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
import { HttpClient, HttpStatusCode, HttpResponse, HttpHeaders, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { AppRoutes } from 'app/app.routes';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { List, Dictionary } from 'ts-generic-collections-linq';
import { ModalDialogComponent } from 'app/components/modal-dialog/modal-dialog.component';
import { AppConfigService } from 'app/services/app-config.service';

// -- import data structure
import {
  NewFileRequest,
  CSRSAccount,
  ModelFile,
  FileStatus,
  AccountFileSummary
} from 'app/api/model/models';
@Component({
  selector: 'app-welcome-user',
  templateUrl: './welcome-user.component.html',
  styleUrls: ['./welcome-user.component.scss']
})
export class WelcomeUserComponent implements OnInit {

  accountFormGroup: FormGroup;
  csrsAccount: CSRSAccount = null;
  data: any = null;
  outData: NewFileRequest = null;
  errorMessage: any = '';
  public cscLink: string;


  constructor(private _formBuilder: FormBuilder, private http: HttpClient,
              @Inject(AccountService) private accountService,
              @Inject(LoggerService) private logger,
              @Inject(Router) private router,
              @Inject(AppConfigService) private appConfigService,
              public dialog: MatDialog,
              private route: ActivatedRoute) {}

  ngOnInit(): void {

    this.cscLink = this.appConfigService.appConfig.cscLink;
    this.logger.info('cscLink :',this.cscLink);

    this.accountFormGroup = this._formBuilder.group({
      fileNumber: ['', Validators.required],
      password: ['', Validators.required]
    });

    this.errorMessage = 'Error: Field is required.';

    //this.logger.info("before accountService.apiAccountGet");

    this.accountService.apiAccountGet().subscribe({
      next: (data:any) => {
        this.logger.info("data:", data);
        if (data)
        {
          var user   = data.user;
          var files  = data.files;

          if (user != null && files != null && files.length > 0)
          {
            const listFiles = new List<ModelFile>(files); //this.logger.info("listFiles", listFiles);
            const activeStatus:ModelFile = listFiles.firstOrDefault(x=>x.status == FileStatus.Active);
            //this.logger.info("activeStatus", activeStatus);

            if (activeStatus)
            {
              //this.logger.info("redirect to Communication");
              this.router.routeReuseStrategy.shouldReuseRoute = () => false;
              this.router.navigate(['/communication']);
            }
          }
        }
      },
      error: (e) => {
        if(e.error instanceof Error)
        {
          //this.logger.info('e.error', e.error);
        }

      },
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
      title: 'Error.',
      content: 'The information you entered is not valid. Please enter the information given to you by the Child Support Recalculation Service. ',
      content_normal: 'If you continue to have problems, contact us at ',
      content_link: '1-866-660-2684.',
      weight: 'bold',
      color: 'red'
    };

    const accountData = this.accountFormGroup.value;
    const csrsAccount: CSRSAccount = {fileNumber: accountData.fileNumber, referenceNumber: accountData.password };

    this.accountService.apiAccountCheckcsrsaccountPost(csrsAccount).subscribe({
      next: (outData:any) => {
        var partyId = outData.partyId;
        var fileId = outData.fileId;

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
      },
    })
  }
}
