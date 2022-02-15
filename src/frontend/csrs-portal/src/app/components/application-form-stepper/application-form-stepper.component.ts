import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
  FormArray,
} from '@angular/forms';
import { HttpClient, HttpStatusCode, HttpResponse } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { JsonPipe } from '@angular/common';
import { AccountService } from 'app/api/api/account.service';
import { LookupService } from 'app/api/api/lookup.service';
import { Inject } from '@angular/core';
import { LoggerService } from '@core/services/logger.service';
import { of } from 'rxjs';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '@shared/dialogs/confirm-dialog/confirm-dialog.component';
import { List, Dictionary } from 'ts-generic-collections-linq';
import { ModalDialogComponent } from 'app/components/modal-dialog/modal-dialog.component';
import { DialogOptions } from '@shared/dialogs/dialog-options.model';
import { Router, ActivatedRoute } from "@angular/router";

// -- import data structure
import {
  NewFileRequest,
  PartyRole,
  FileStatus,
  Child,
  LookupValue,
  CourtLookupValue,
  CSRSPartyFileIds,
  CSRSAccount,
  CSRSAccountRequest,
  Party,
  CSRSAccountFile
  } from 'app/api/model/models';

import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-application-form-stepper',
  templateUrl: './application-form-stepper.component.html',
  styleUrls: ['./application-form-stepper.component.scss']
})
export class ApplicationFormStepperComponent implements OnInit {

  secondFormGroup: FormGroup;
  sixFormGroup: FormGroup;
  eFormGroup: FormGroup;
  nineFormGroup: FormGroup;

  provinces: any = [];
  genders: any = [];
  identities: any = [];
  referrals: any = [];
  preferredContactMethods: any = [];

   isEditable = false;

  _yes: number = 867670000;
  _no: number = 867670001;
  _iDontKnow: number = 867670002;

  data: any = null;

  csrsPartyFileIds: CSRSPartyFileIds = null;
  csrsAccount: CSRSAccount = null;
  csrsAccountRequest: CSRSAccountRequest = null;
  party: Party = null;
  csrsAccountFile: CSRSAccountFile = null;

  partyId: any = '';
  fileId: any = '';


  constructor(private _formBuilder: FormBuilder, private http: HttpClient,
      @Inject(AccountService) private accountService,
      @Inject(LookupService) private lookupService,
      @Inject(LoggerService) private logger,
      @Inject(OidcSecurityService) private oidc,
      public dialog: MatDialog,
      private datePipe: DatePipe,
      private route: ActivatedRoute) {
        this.partyId = this.route.snapshot.paramMap.get('partyId');
        this.fileId = this.route.snapshot.paramMap.get('partyId');
      }

  ngOnInit() {

    this.provinces = [{id: '123', value: 'British Columbia'}];
    this.identities = [{id: '123', value: 'Native'}];
    this.genders =  [{id: '123', value: 'Male'}];
    this.referrals = [{id: '123', value: 'FMEP'}];

    this.getReferrals();
    this.getIdentities();
    this.getProvinces();
    this.getGenders();
    this.getPreferredcontactmethods();
    // --- check account


    this.secondFormGroup = this._formBuilder.group({
      firstName: ['', Validators.required],
      givenNames: [''],
      lastName: ['', Validators.required],
      birthdate: ['', Validators.required],
      address1: ['', Validators.required],
      city: ['', Validators.required],
      province: ['', Validators.required],
      postalCode: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      email: ['', Validators.email],
      PreferredName: [''],
      saddress: [''],
      cellNumber: [''],
      workNumber: [''],
      gender: [''],
      identity: ['']
    });

    this.sixFormGroup = this._formBuilder.group({
      // secondCtrl: ['', Validators.required],
      childSafety: [''],
      contactMethod: [''],
      enrollFMEP: [''],
      FMEPinput: [''],
      incomeAssistance: [''],
      referral: [''],

    });
    this.eFormGroup = this._formBuilder.group({
      secondCtrl: ['', Validators.required],
    });
    this.nineFormGroup = this._formBuilder.group({
      secondCtrl: [''],
    });
    this.setFormDataFromLocal();
  }
  setFormDataFromLocal(){
  if (localStorage.getItem('formData')){
      let data = localStorage.getItem('formData');
      data = JSON.parse(data);
      if (data['secondFormGroup']){
        this.secondFormGroup.patchValue(data['secondFormGroup']);
      }
      if (data['sixFormGroup']){
        this.sixFormGroup.patchValue(data['sixFormGroup']);
      }
      if (data['eFormGroup']){
        this.eFormGroup.patchValue(data['eFormGroup']);
      }
      if (data['nineFormGroup']){
        this.nineFormGroup.patchValue(data['nineFormGroup']);
      }
  }
}

  openDialog(inData) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent,{
      width: '550px',
      data: inData
    });

    dialogRef.afterClosed().subscribe(result => {
      this.logger.log(`Dialog result: ${result}`);
    });
  }
  editPage(stepper, index){
    this.isEditable = true;
    stepper.selectedIndex = index;
  }

  getIdentities() {
    this.accountService.configuration.accessToken =  this.oidc.getAccessToken();
    this.accountService.apiAccountIdentitiesGet().subscribe({
        next: (data) => {
          this.identities = data;
          this.logger.info('this.identities',this.identities);
        },
        error: (e) => {
          if (e.error instanceof Error) {
            this.logger.error(e.error.message);
          } else {
              // Backend returns unsuccessful response codes such as 404, 500 etc.
              this.logger.info('Backend returned ', e);
            }
        },
        complete: () => this.logger.info('apiAccountIdentitiesGet is completed')
    });
  }

  getProvinces() {
    this.accountService.configuration.accessToken =  this.oidc.getAccessToken();
    this.accountService.apiAccountProvincesGet().subscribe({
      next: (data) => {
        this.provinces = data;
        this.logger.info('this.provinces',this.provinces);
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this.logger.error(e.error.message);
        } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this.logger.info('Backend returned ', e);
          }
      },
      complete: () => this.logger.info('apiAccountProvincesGet is completed')
    })
  }

  getGenders() {
    this.accountService.configuration.accessToken =  this.oidc.getAccessToken();
    this.accountService.apiAccountGendersGet().subscribe({
      next: (data) => {
        this.genders = data;
        this.logger.info('this.genders',this.genders);
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this.logger.error(e.error.message);
        } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this.logger.info('Backend returned ', e);
          }
      },
      complete: () => this.logger.info('apiAccountGendersGet is completed')
    })
  }

  getReferrals() {
    this.accountService.configuration.accessToken =  this.oidc.getAccessToken();
    this.accountService.apiAccountReferralsGet().subscribe({
      next: (data) => {
        this.referrals = data;
        this.logger.info('this.referals',this.referrals);
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this.logger.error(e.error.message);
        } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this.logger.info('Backend returned ', e);
          }
      },
      complete: () => this.logger.info('apiAccountReferralsGet is completed')
    })
  }

  getPreferredcontactmethods(){
    this.accountService.configuration.accessToken =  this.oidc.getAccessToken();
    this.accountService.apiAccountPreferredcontactmethodsGet().subscribe({
      next: (data) => {
        this.preferredContactMethods = data;
        this.logger.info('this.preferredContactMethods',this.preferredContactMethods);
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this.logger.error(e.error.message);
        } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this.logger.info('Backend returned ', e);
          }
      },
      complete: () => this.logger.info('apiAccountReferralsGet is completed')
    })
  }

  saveLater(){
    const formData = {
      secondFormGroup: this.secondFormGroup.value,
      sixFormGroup: this.sixFormGroup.value,
      eFormGroup: this.eFormGroup.value,
      nineFormGroup: this.nineFormGroup.value,
    };

    this.logger.info("formData", formData);
    this.prepareData();
    localStorage.setItem('formData', JSON.stringify(formData));
  }
  save(){
    localStorage.getsetItemItem('formData', '');
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


  transformDate(date) {
    return this.datePipe.transform(date, 'yyyy-MM-dd');
  }

  findId(value){
    if (value == 'Yes'){
      return this._yes;
    }
    else
      if (value == 'No'){
        return this._no;
      }
    return this._iDontKnow;
  }

  prepareData(){

      // --- populate party
      const partyData = this.secondFormGroup.value;
      const file2Data = this.sixFormGroup.value;

      //let LookupValue
      let listGender = new List<LookupValue>(this.genders);
      let inGender: LookupValue = listGender.firstOrDefault(x=>x.id == partyData.gender);

      let listProvince = new List<LookupValue>(this.provinces);
      let inProvince: LookupValue = listProvince.firstOrDefault(x=>x.id == partyData.province);

      let listIdentityParty = new List<LookupValue>(this.identities);
      let inIdentityParty: LookupValue = listIdentityParty.firstOrDefault(x=>x.id == partyData.identity);

      let listReferral = new List<LookupValue>(this.referrals);
      let inReferral: LookupValue = listReferral.firstOrDefault(x=>x.id == file2Data.referral);

      let list = new List<LookupValue>(this.preferredContactMethods);
      let inPreferredContactMethod: LookupValue = list.firstOrDefault(x=>x.value == file2Data.contactMethod);

      let inParty: Party = {
          partyId: '00000000-0000-0000-0000-000000000000',
          firstName: partyData.firstName,
          middleName: partyData.givenNames,
          lastName: partyData.lastName,
          preferredName: partyData.PreferredName,
          dateOfBirth: this.transformDate(partyData.birthdate),
          gender: inGender,
          addressStreet1: partyData.address1,
          addressStreet2: partyData.saddress,
          city: partyData.city,
          province: inProvince,
          postalCode: partyData.postalCode,
          homePhone: partyData.phoneNumber,
          workPhone: partyData.workNumber,
          cellPhone: partyData.cellNumber,
          email: partyData.email,
          optOutElectronicDocuments: null,   // ??? may need to remove?
          identity: inIdentityParty,
          referral: inReferral,
          preferredContactMethod: inPreferredContactMethod,
          incomeAssistance: this.findId(file2Data.incomeAssistance),
          //referenceNumber = null
      }

      // --- populate file

      let inFile:any = {
          status: FileStatus.Unknown,
          //usersRole: partyRole,
          fileId: '0',
          fileNumber: null,
          //partyEnrolled: partyEnrolled,
          //courtFileType: inCourtFileType,
          //bcCourtLevel: inBcCourtLevel,
          //bcCourtLocation: inBcCourtLocation,
          //dateOfOrderOrWA: this.transformDate(file1Data.orderDate),
          //incomeOnOrder: file1Data.payorIncome,
          //section7Expenses: file1Data.isSpecifiedIncome,
          safetyAlertRecipient: null,
          recipientSafetyConcernDescription: null,
          safetyAlertPayor: null,
          payorSafetyConcernDescription: null,
          isFMEPFileActive: file2Data.enrollFMEP,
          fmepFileNumber: file2Data.FMEPinput,
          //recalculationOrderByCourt: file1Data.recalculationOrdered,
          //otherParty: inOtherParty,
          //children: childs
      }

      // --- populate
      let newFileRequest: NewFileRequest = {
        user: inParty,
        file: inFile,
      }

      /*
      if (partyEnrolled == 'Recipient')
      {
        newFileRequest.file.safetyAlertRecipient = file2Data.childSafety;
        newFileRequest.file.recipientSafetyConcernDescription = file2Data.childSafetyDescription;
      }
      else
      {
        newFileRequest.file.safetyAlertPayor = file2Data.childSafety;
        newFileRequest.file.payorSafetyConcernDescription = file2Data.childSafetyDescription;
      }*/

      this.logger.info("newFileRequest:", newFileRequest);

    //this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this.accountService.apiAccountCreatePost(newFileRequest).subscribe({
      next: (outData:any) => {

        var partyId = outData.partyId;
        var fileId = outData.fileId;
        var fileNumber = outData.fileNumber;

        this.logger.info("partyId", partyId);
        this.logger.info("fileId", fileId);
        this.logger.info("fileNumber", fileNumber);

        let customOptions: DialogOptions = { data: {fileNumber: fileNumber}};
        this.openDialog(customOptions);
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this.logger.error(e.error.message);

          this.data = {
            type: 'error',
            title: e.error.message,
            content: '',
            weight: 'normal',
            color: 'red'
          };
          this.openModalDialog();

        } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this.logger.info('Backend returned ', e);
          }
      },
      complete: () => this.logger.info('apiAccountCreatePost is completed')
    })

  }

}
