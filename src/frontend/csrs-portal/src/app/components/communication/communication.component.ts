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
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDialogComponent } from '@shared/dialogs/confirm-dialog/confirm-dialog.component';
import { DatePipe } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { AccountService } from 'app/api/api/account.service';
import { AccountFileSummary } from 'app/api/model/accountFileSummary.model';
import { UserRequestService } from 'app/api/api/userRequest.service';
import { DocumentService } from 'app/api/api/document.service';
import { UserRequest } from '../../api';
import { Router, ActivatedRoute } from "@angular/router";
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
              @Inject(AccountService) private accountService,
              @Inject(UserRequestService) private userRequestService,
              @Inject(DocumentService) private documentService,
              private _http: HttpClient,
              public dialog: MatDialog,
              private datePipe: DatePipe,
              private route: ActivatedRoute,
              @Inject(Router) private router  ) {
   }
  showValidationMessages: boolean;
  validationMessages: any[];
  uploadFormGroup: FormGroup;
  bceIdLink: string;
  selectedFile: File = null;
  documentTypes: any = [];
  // isDisabled: boolean = true;
  // isUploaing: boolean = true;
  _token = '';
  selectedUploadFile: any;

  data: any = null;
  _reponse: HttpResponse<null>;
  contactFormGroup: FormGroup;
  files: any[];
  selectedContactFile: any;
  curDate = new Date();
  curDateStr: string;
  portalUser: string;
  contactSubjects = [
    { id: 1, name: "Request a call" },
    { id: 2, name: "File withdrawal" },
    { id: 3, name: "Section 7 expenses withdrawal" },
    { id: 4, name: "Clerical error on the Statement of Recalculation" },
    { id: 5, name: "Safety concerns" },
    { id: 6, name: "Update contact information" },
    { id: 7, name: "Change preferred method of communication" },
    { id: 8, name: "Other" }
  ];

  accountSummary: HttpResponse<AccountFileSummary>;
  public toggleRow = false;
  selectedTab: number = 0;
  selectedFileNumber: any = '';

  ngOnInit(): void {

    this.route.queryParams
    .subscribe(params => {
      this.logger.info("params", params);
      this.selectedTab = params.index;
      this.selectedFileNumber = params.fileNumber;
    });

    this.curDateStr = this.datePipe.transform(this.curDate, 'yyyy-MM-dd');
    this.accountService.apiAccountGet('response', false).subscribe({
      next: (data) => {
        this.accountSummary = data;
        this.portalUser = this.accountSummary.body.user.firstName;
        if ( this.accountSummary.status === HttpStatusCode.Ok &&
              (this.accountSummary.body.files != null || this.accountSummary.body.files.length > 0)) {
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
      uploadFile: [null, Validators.required],
      documentType: [null, Validators.required],
      secondCtrl: [''],
    });

    this.documentTypes = [
      { id: 'Order_or_Written_Agreement', docType: 'Order or Written Agreement'},
      { id: 'Notice_of_Assessment', docType: 'Notice of Assessment'},
      { id: 'Income_Tax_Return', docType: 'Income Tax Return'},
      { id: 'Court_Application', docType: 'Court Application'},
      {id: 'Other', docType: 'Other'},
    ];

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
  get uploadFile() {
    return this.uploadFormGroup.get('uploadFile');
  }
  get documentType() {
    return this.uploadFormGroup.get('documentType');
  }
  onContactFileNumberChange(ob): void {
    let fileValue = ob.value;
    for (var i = 0; i < this.files.length; i++) {
      if (fileValue == this.files[i].fileId) {
        this.selectedContactFile = this.files[i];
      }
    }
  }
  onUploadFileNumberChange(ob): void {
    let fileValue = ob.value;
    for (var i = 0; i < this.files.length; i++) {
      if (fileValue == this.files[i].fileId) {
        this.selectedUploadFile = this.files[i];
      }
    }
  }
  clearContactForm(): void {
    this.showValidationMessages = false;
    this.validationMessages = [];
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
    if (!this.contactFormGroup.valid) {
      this.validationMessages = [];
      this.showValidationMessages = true;
      if (this.contactFile.hasError) {
        for (const error in this.contactFile.errors) {
          this.validationMessages.push('Contact file '+error.toString());
        }
      }
      if (this.contactSubject.hasError) {
        for (const error in this.contactSubject.errors) {
          this.validationMessages.push('Contact Subject ' + error.toString());
        }
      }
      if (this.contactMessage.hasError) {
        for (const error in this.contactMessage.errors) {
          this.validationMessages.push('Contact Message ' + error.toString());
        }
      }
    } else {
      this.showValidationMessages = false;
      this.validationMessages = [];
      this.showValidationMessages = false;
      let createUserRequest: UserRequest = {
        fileId: this.selectedContactFile.fileId,
        fileNo: this.selectedContactFile.fileNumber,
        requestType: this.contactSubject.value,
        requestMessage: this.contactMessage.value
      }
      this.userRequestService.apiUserrequestCreatePost(createUserRequest).subscribe({
        next: (outData: any) => {
          this._reponse = outData;


            this.data = {
              type: 'info',
              title: 'Contact Request Created',
              content: 'File #: ' + this.selectedContactFile.fileNumber,
              weight: 'bold',
              color: 'green'
            };
            this.openDialog();
        },
        error: (e) => {
          if (e.error instanceof Error) {
            this.logger.error(e.error.message);

            this.data = {
              type: 'error',
              title: 'Error encountered',
              content: e.error.message,
              weight: 'normal',
              color: 'red'
            };
            this.openDialog();

          } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this.logger.info('Backend returned ', e);
            this.data = {
              type: 'error',
              title: 'Contact Request Failed',
              content: e.error.message,
              weight: 'normal',
              color: 'red'
            };
            this.openDialog();
          }
        },
        complete: () => this.logger.info('apiUserrequestCreatePost is completed')
      })
      this.clearContactForm();

    }
  }

  ontable(element){
    console.log('>>>', element);
    this.toggleRow = element;
  }
  onUpload(): void {
    this.validationMessages = [];
    this.showValidationMessages = false;
    if (this.uploadFormGroup.valid) {
      if (this.selectedFile !== null) {
        this.submitUploadedAttachment();
        this.selectedFile = null;
      } else {
        this.validationMessages = [];
        this.showValidationMessages = true;
        this.validationMessages.push('A Document Must be Provided');
      }
    } else {
      this.validationMessages = [];
      this.showValidationMessages = true;
      if (this.uploadFile.hasError) {
        for (const error in this.uploadFile.errors) {
          this.validationMessages.push('Upload file ' + error.toString());
        }
      }
      if (this.documentType.hasError) {
        for (const error in this.documentType.errors) {
          this.validationMessages.push('Document Type ' + error.toString());
        }
      }
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

submitUploadedAttachment() {
    const fileData = new FormData();
    fileData.append('file', this.selectedFile, this.selectedFile.name);
    this.logger.info('File Data', fileData);

      this.documentService.apiDocumentUploadattachmentPost(
        this.selectedUploadFile.fileId,
        "ssg_csrsfile",
        this.documentType.value,
        this.selectedFile
      ).subscribe({
      next:  (data) => {
        this._reponse = data;
        this.data = {
            type: 'info',
            title: 'Success - document uploaded',
            content: 'Document uploaded to file #: ' + this.selectedUploadFile.fileNumber,
            weight: 'bold',
            color: 'green'
          };
          this.openDialog();
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this.logger.error(e.error.message);
          this.data = {
            type: 'error',
            title: 'Error Encountered',
            content: e.error.message,
            weight: 'normal',
            color: 'red'
          };
          this.openDialog();

        } else {
            // Backend returns unsuccessful response codes such as 404, 500 etc.
          this.logger.info('Backend returned ', e);
          this.data = {
            type: 'error',
            title: 'Upload Failed',
            content: e.error.message,
            weight: 'normal',
            color: 'red'
          };
          this.openDialog();
          }
      },
      complete: () => this.logger.info('apiFileUploadattachmentPost is completed')
      });

  }
}
