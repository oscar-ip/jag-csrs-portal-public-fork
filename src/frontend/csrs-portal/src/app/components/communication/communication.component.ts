import { Component, OnInit, ViewChild, ViewChildren, ElementRef } from '@angular/core';
import { Inject } from '@angular/core';
import { LoggerService } from '@core/services/logger.service';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
  FormArray,
} from '@angular/forms';
import { HttpClient, HttpErrorResponse, HttpEventType, HttpStatusCode, HttpResponse, HttpHeaders  } from '@angular/common/http';
import { AppConfigService } from 'app/services/app-config.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogComponent } from 'app/components/modal-dialog/modal-dialog.component';
import { FileService } from 'app/api/api/file.service';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDialogComponent } from '@shared/dialogs/confirm-dialog/confirm-dialog.component';
import { DatePipe } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { AccountService } from 'app/api/api/account.service';
import { AccountFileSummary } from 'app/api/model/accountFileSummary.model';

@Component({
  selector: 'app-communication',
  templateUrl: './communication.component.html',
  styleUrls: ['./communication.component.scss'],
  providers: [DatePipe]
})

export class CommunicationComponent implements OnInit {
  dataSource = new MatTableDataSource();
  displayedColumns: string[] = ['id', 'name', 'username', 'email', 'phone'];

  constructor(private _formBuilder: FormBuilder,
              @Inject(LoggerService) private logger,
              @Inject(AppConfigService) private appConfigService,
              @Inject(FileService) private fileService,
              @Inject(OidcSecurityService) private oidc,
              @Inject(AccountService) private accountService,
              private _http: HttpClient,
              public dialog: MatDialog,
              private datePipe: DatePipe  ) {
   }

  uploadFormGroup: FormGroup;
  bceIdLink: string;
  selectedFile: File = null;
  documentTypes: any = [];
  // isDisabled: boolean = true;
  // isUploaing: boolean = true;
  _token = '';

  data: any = null;
  selectedDocumentType = '';
  _reponse: HttpResponse<null>;
  contactFormGroup: FormGroup;
  files: any[];
  curDate = new Date();
  curDateStr: string
  contactSubjects = [
    { id: 1, name: "Subject One" },
    { id: 2, name: "Subject Two" },
    { id: 3, name: "Subject Three" },
    { id: 4, name: "Subject Four" },
    { id: 5, name: "Other" }
  ];
  
  accountSummary: HttpResponse<AccountFileSummary>;
  public toggleRow = false;
  ngOnInit(): void {
    this.curDateStr = this.datePipe.transform(this.curDate, 'yyyy-MM-dd');
    console.log('Account Get');
    this.accountService.apiAccountGet('response', false).subscribe({
      next: (data) => {
        this.accountSummary = data;
        console.log('Files size' + this.accountSummary.body.files.length);
        if ( this.accountSummary.status === HttpStatusCode.Ok &&
              (this.accountSummary.body.files != null || this.accountSummary.body.files.length > 0)) {
          console.log('Files size' + this.accountSummary.body.files.length);
          this.files = this.accountSummary.body.files;
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
    this.getRemoteData();
    this.uploadFormGroup = this._formBuilder.group({
      secondCtrl: [''],
    });

    this.documentTypes = [
      {id: '1', docType: 'Order or Written Agreement'},
      {id: '2', docType: 'Notice of Assessment'},
      {id: '3', docType: 'Income Tax Return'},
      {id: '4', docType: 'Court Application'},
      {id: '5', docType: 'Other'},
    ];

    this._token = '';

    this.contactFormGroup = this._formBuilder.group({
      contactFile: [null, Validators.required],
      contactSubject: [null, Validators.required],
      contactMessage: [null, [Validators.required, Validators.maxLength(500)]]
    });
  }
  get contactFile() {
    return this.contactFormGroup.get('contactFile');
  }
  get contactDate() {
    return this.curDateStr;
  }
  get contactSubject() {
    return this.contactFormGroup.get('contactSubject');
  }
  get contactMessage() {
    return this.contactFormGroup.get('contactMessage');
  }
  onFileChange(ob) {
    console.log('File changed...');
    let file = ob.value;
    console.log(file);
  }
  clearContactForm(): void {
    console.log('Form reset...');
    this.contactFormGroup.reset();
  }  

  getRemoteData() {

    const remoteDummyData = [
      {
        date: '2021/1/1',
        file: 2453,
        subject: 'the notice of assement for 2021 is avaiable for review',
        attachment: 'PDF',
        link: 'http://www.africau.edu/images/default/sample.pdf',
      },
      {
        date: '2020/2/1',
        file: 5443,
        subject: 'the notice of assement for 2021 is avaiable for review',
        attachment: 'PDF',
        link: 'http://www.africau.edu/images/default/sample.pdf',
      },
      {
        date: '2019/3/1',
        file: 8754,
        subject: 'the notice of assement for 2021 is avaiable for review',
        attachment: 'PDF',
        link: 'http://www.africau.edu/images/default/sample.pdf',
      },
      
    ];
    this.dataSource.data = remoteDummyData;
    }
    
  sendContact(): void {
    if (this.contactFormGroup.valid) {

    } else {
      console.log('Form invalid...');
    }
  }


  ontable(element){
    console.log('>>>', element);
    this.toggleRow = element;
  }
onUpload(): void {

    if (this.selectedFile !== null)
{

      this.submitUploadedAttachment();

      this.data = {
          type: 'info',
          title: 'Success - document uploaded',
          content: 'Document uploaded to file #: 4652',
          weight: 'bold',
          color: 'green'
        };

      this.openDialog();
      this.selectedFile = null;
      // this.blob = null;
    }
  }

onFileSelected(event) {
    this.selectedFile = event.target.files[0];
    this.logger.info('Selected File', this.selectedFile);

    // this.isDisabled = true;
    // this.isUploaing = false;

    if (this.selectedFile.type !== 'application/pdf' &&
        this.selectedFile.type !== 'image/gif' &&
        this.selectedFile.type !== 'image/jpg' &&
        this.selectedFile.type !== 'image/jpeg' &&
        this.selectedFile.type !== 'image/png')
    {
      this.selectedFile = null;
      this.data = {
        type: 'error',
        title: 'Selected file is not a supported file type.',
        content: 'Supported file types are .pdf, .gif, .jpg, .jpeg, .png.',
        weight: 'normal',
        color: 'red'
      };
      this.openDialog();
    }

    if (this.selectedFile.size >= 10000000)
    {
      this.selectedFile = null;
      this.data = {
        type: 'error',
        title: 'Selected file is over the maximum supported file size.',
        content: 'Maximum supported file size is 10MB.',
        weight: 'normal',
        color: 'red'
      };
      this.openDialog();
    }

  }

onDeleteFile() {
    this.selectedFile = null;
    // this.isDisabled = false;
    // this.isUploaing = true;
  }

openDialog(): void {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '450px',
      data: this.data,
    });

    dialogRef.afterClosed().subscribe(result => {
      this.logger.info('Modal dialog was closed');
    });
  }
  openConfirmationDialog() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent,{
      width: '550px'
    });
  
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

onDocTypeChanged(event) {
    this.selectedDocumentType = event.target.value;
    this.logger.info('selectedDocumentType', this.selectedDocumentType);
  }

submitUploadedAttachment() {
    this.fileService.configuration.accessToken = this.oidc.getAccessToken();

    const httpOptions = {
    headers: new HttpHeaders({'Content-Type': this.selectedFile.type})
  };

    const fileData = new FormData();
    fileData.append('file', this.selectedFile, this.selectedFile.name);
    this.logger.info('File Data', fileData);

    this.fileService.apiFileUploadattachmentPost(
      'EDE069F4-0E21-4AAD-AAB1-198C195A08BC',
      this.selectedDocumentType,
      fileData,
      'body'
      ).subscribe({
      next:  (data) => {
        this._reponse = data;
        if ( this._reponse.status === HttpStatusCode.Ok ) {
          this.logger.info('_reponse.status = HttpStatusCode.Ok');

        }
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this.logger.error(e.error.message);
        } else {
            // Backend returns unsuccessful response codes such as 404, 500 etc.
            this.logger.info('Backend returned ', e);
          }
      },
      complete: () => this.logger.info('apiFileUploadattachmentPost is completed')
    });

  }
}
