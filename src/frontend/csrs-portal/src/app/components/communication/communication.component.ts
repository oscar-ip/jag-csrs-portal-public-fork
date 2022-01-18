import { Component, OnInit, ViewChild, ViewChildren, ElementRef } from "@angular/core";
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

@Component({
  selector: 'app-communication',
  templateUrl: './communication.component.html',
  styleUrls: ['./communication.component.scss']
})

export class CommunicationComponent implements OnInit {

  uploadFormGroup: FormGroup;
  bceIdLink: string;
  selectedFile:File = null;
  documentTypes: any = [];
  //isDisabled: boolean = true;
  //isUploaing: boolean = true;
  _token: string = '';

  data: any = null;
  selectedDocumentType: string = '';
  _reponse: HttpResponse<null> ;

  constructor(private _formBuilder: FormBuilder,
              @Inject(LoggerService) private logger,
              @Inject(AppConfigService) private appConfigService,
              @Inject(FileService) private fileService,
              @Inject(OidcSecurityService) private oidc,
              private _http: HttpClient,
              public dialog: MatDialog) {
   }

  ngOnInit(): void {
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
      //this.blob = null;
    }
  }

  onFileSelected(event) {
    this.selectedFile = event.target.files[0];
    this.logger.info("Selected File", this.selectedFile);

    //this.isDisabled = true;
    //this.isUploaing = false;

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
    //this.isDisabled = false;
    //this.isUploaing = true;
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '450px',
      data: this.data,
    });

    dialogRef.afterClosed().subscribe(result => {
      this.logger.info("Modal dialog was closed");
    });
  }

  onDocTypeChanged(event) {
    this.selectedDocumentType = event.target.value;
    this.logger.info("selectedDocumentType", this.selectedDocumentType);
  }

  submitUploadedAttachment() {
    this.fileService.configuration.accessToken = this.oidc.getAccessToken();

  const httpOptions = {
    headers: new HttpHeaders({'Content-Type': this.selectedFile.type})
  };

    let fileData = new FormData();
    fileData.append("file", this.selectedFile,this.selectedFile.name);
    this.logger.info('File Data',fileData);

    this.fileService.apiFileUploadattachmentPost(
      'EDE069F4-0E21-4AAD-AAB1-198C195A08BC',
      this.selectedDocumentType,
      fileData,
      'body'
      ).subscribe({
      next:  (data) => {
        this._reponse = data;
        if ( this._reponse.status === HttpStatusCode.Ok ) {
          this.logger.info("_reponse.status = HttpStatusCode.Ok")

        }
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this.logger.error(e.error.message);
        } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this.logger.info('Backend returned ', e);
          }
      },
      complete: () => this.logger.info('apiFileUploadattachmentPost is completed')
    })

  }

}
