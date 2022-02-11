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
import {MatDialog} from '@angular/material/dialog';
import { ConfirmDialogComponent } from '@shared/dialogs/confirm-dialog/confirm-dialog.component';

// -- import data structure
import {
  NewFileRequest,
  PartyRole,
  FileStatus,
  Party,
  Child,
  LookupValue,
  CourtLookupValue
  } from 'app/api/model/models';

import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-child-application-question',
  templateUrl: './child-application-question.component.html',
  styleUrls: ['./child-application-question.component.scss'],
})
export class ChildApplicationQuestionComponent implements OnInit {
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  thirdFormGroup: FormGroup;
  fourthFormGroup;
  fourthFormGroup1: FormGroup;
  fifthFormGroup: FormGroup;
  sixFormGroup: FormGroup;
  seventhFormGroup: FormGroup;
  eFormGroup: FormGroup;
  nineFormGroup: FormGroup;
  provinceData: any = [];
  genderData: any = [];
  identityData: any = [];
  referalsData: any = [];
  courtLocationsData: any = [];
  _accountService: AccountService;
  _lookupService: LookupService;
  _logger: LoggerService;
  _oidc: OidcSecurityService;
  today = new Date();
  isEditable: boolean = false;
  child: Child;
  _reponse: HttpResponse<any>;

  constructor(private _formBuilder: FormBuilder, private http: HttpClient,
              @Inject(AccountService) private accountService,
              @Inject(LookupService) private lookupService,
              @Inject(LoggerService) private logger,
              @Inject(OidcSecurityService) private oidc,
              public dialog: MatDialog,
              private datePipe: DatePipe) {}

  ngOnInit() {
    this._accountService = this.accountService;
    this._lookupService = this.lookupService;
    this._logger = this.logger;
    this._oidc = this.oidc;

    this.provinceData = [{label: 'province', value: 'British Columbia'}];
    this.identityData = [{label: 'identity', value: 'Native'}];
    this.genderData =  [{label: 'gender', value: 'Male'}];
    this.courtLocationsData =  [{label: 'courtLocation', value: 'Victoria Court'}];
    this.referalsData = [{label: 'referal', value: 'FMEP'}];

    this.getReferalsData();
    this.getCourtLocationData();
    this.getIdentityData();
    this.getProvinceData();
    this.getGenderyData();

    this.firstFormGroup = this._formBuilder.group({
      firstControl: ['', Validators.required],
      secondControl: ['', Validators.required]
    });
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
    this.thirdFormGroup = this._formBuilder.group({
      firstName: ['', Validators.required],
      givenNames: [''],
      lastName: ['', Validators.required],
      pname: [''],
      birthdate: [''],
      saddress1: [''],
      saddress2: [''],
      city: [''],
      province: [''],
      postalCode: [''],
      homePhoneNumber: [''],
      cellPhoneNumber: [''],
      workPhoneNumber: [''],
      email: ['', Validators.required],
      gender: ['']

    });

    this.fourthFormGroup1 = this._formBuilder.group({
      users: this._formBuilder.array([
        this._formBuilder.group({
          firstName: [''],
          lastName: [''],
          birthdate: [],
          childDependency: [],
          middleName: []
        })

      ])
    });
    // this.fourthFormGroup = this._formBuilder.group({
    //   firstName: ['', Validators.required],
    //   lastName: ['', Validators.required],
    //   birthdate: [],
    //   givenNames: [],
    //   middleName: []
    // });
    this.fifthFormGroup = this._formBuilder.group({
      orderDate: [],
      courtLocation: [],
      payorIncome: [],
      recalculationOrdered: [],
      isSpecifiedIncome: [],
    });
    this.sixFormGroup = this._formBuilder.group({
      // secondCtrl: ['', Validators.required],
      childSafety: [''],
      childSafetyDescription: [''],
      contactMethod: [''],
      enrollFMEP: [''],
      FMEPinput: [''],
      incomeAssistance: [''],
      referals: [''],

    });
    this.seventhFormGroup = this._formBuilder.group({
      secondCtrl: [''],
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
      if (data['firstStep']){
        this.firstFormGroup.patchValue(data['firstStep']);
      }
      if (data['secondFormGroup']){
        this.secondFormGroup.patchValue(data['secondFormGroup']);
      }
      if (data['thirdFormGroup']){
        this.thirdFormGroup.patchValue(data['thirdFormGroup']);
      }
      if (data['fourthFormGroup']){
        this.fourthFormGroup.patchValue(data['fourthFormGroup']);
      }
      if (data['fifthFormGroup']){
        this.fifthFormGroup.patchValue(data['fifthFormGroup']);
      }
      if (data['sixFormGroup']){
        this.sixFormGroup.patchValue(data['sixFormGroup']);
      }
      if (data['seventhFormGroup']){
        this.seventhFormGroup.patchValue(data['seventhFormGroup']);
      }
      if (data['eFormGroup']){
        this.eFormGroup.patchValue(data['eFormGroup']);
      }
      if (data['nineFormGroup']){
        this.nineFormGroup.patchValue(data['nineFormGroup']);
      }
  }

}

openDialog() {
  const dialogRef = this.dialog.open(ConfirmDialogComponent,{
    width: '550px'
  });

  dialogRef.afterClosed().subscribe(result => {
    console.log(`Dialog result: ${result}`);
  });
}

editPage(stepper, index){
  this.isEditable = true;
  stepper.selectedIndex = index;
}
  getIdentityData() {

    //this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountIdentitiesGet().subscribe({
        next: (data) => {
          this.identityData = data;
          this._logger.info('this.identityData',this.identityData);
        },
        error: (e) => {
          if (e.error instanceof Error) {
            this._logger.error(e.error.message);
          } else {
              // Backend returns unsuccessful response codes such as 404, 500 etc.
              this._logger.info('Backend returned ', e);
            }
        },
        complete: () => this._logger.info('apiAccountIdentitiesGet is completed')
    });
  }

  getProvinceData() {
    //this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountProvincesGet().subscribe({
      next: (data) => {
        this.provinceData = data;
        this._logger.info('this.provinceData',this.provinceData);
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this._logger.error(e.error.message);
        } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this._logger.info('Backend returned ', e);
          }
      },
      complete: () => this._logger.info('apiAccountProvincesGet is completed')
    })
  }

  getGenderyData() {
    //this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountGendersGet().subscribe({
      next: (data) => {
        this.genderData = data;
        this._logger.info('this.genderData',this.genderData);
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this._logger.error(e.error.message);
        } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this._logger.info('Backend returned ', e);
          }
      },
      complete: () => this._logger.info('apiAccountGendersGet is completed')
    })
  }

  getCourtLocationData() {
    //this._lookupService.configuration.accessToken =  this._oidc.getAccessToken();
      this._lookupService.apiLookupCourtlocationsGet().subscribe({
        next: (data) => {
          this.courtLocationsData = data;
          this._logger.info('this.courtLocationsData',this.courtLocationsData);
        },
        error: (e) => {
          if (e.error instanceof Error) {
            this._logger.error(e.error.message);
          } else {
              //Backend returns unsuccessful response codes such as 404, 500 etc.
              this._logger.info('Backend returned ', e);
            }
        },
        complete: () => this._logger.info('courtlocationsGet is completed')
      })
  }

  getReferalsData() {
    //this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountReferralsGet().subscribe({
      next: (data) => {
        this.referalsData = data;
        this._logger.info('this.referalsData',this.referalsData);
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this._logger.error(e.error.message);
        } else {
            //Backend returns unsuccessful response codes such as 404, 500 etc.
            this._logger.info('Backend returned ', e);
          }
      },
      complete: () => this._logger.info('apiAccountReferralsGet is completed')
    })
  }

  callAntherchild(){
    const usersArray = this.fourthFormGroup1.controls.users as FormArray;
    const arraylen = usersArray.length;

    const newUsergroup: FormGroup = this._formBuilder.group({
      firstName: [''],
      lastName: [''],
      birthdate: [],
      givenNames: [],
      childDependency: [],
      middleName: []
    });

    usersArray.insert(arraylen, newUsergroup);
  }

  saveLater(){
    const formData = {
      firstStep: this.firstFormGroup.value,
      secondFormGroup: this.secondFormGroup.value,
      thirdFormGroup: this.thirdFormGroup.value,
      fourthFormGroup1: this.fourthFormGroup1.value,
      fifthFormGroup: this.fifthFormGroup.value,
      sixFormGroup: this.sixFormGroup.value,
      seventhFormGroup: this.seventhFormGroup.value,
      eFormGroup: this.eFormGroup.value,
      nineFormGroup: this.nineFormGroup.value,
    };

    this._logger.info("formData", formData);
    this.prepareData();

    localStorage.setItem('formData', JSON.stringify(formData));
  }
  save(){
    this.prepareData();

    localStorage.getsetItemItem('formData', '');
  }

  transformDate(date) {
    return this.datePipe.transform(date, 'yyyy-MM-dd');
  }

  prepareData(){

    // --- populate children
    const users = this.fourthFormGroup1.value.users;
    let childs: Array<Child> = new Array<Child>();

    for(var i = 0; i < users.length;i++) {
      let child: Child = {
        firstName: users[i].firstName,
        middleName: users[i].middleName,
        lastName: users[i].lastName,
        dateOfBirth: this.transformDate(users[i].birthdate),
        childIsDependent: users[i].childDependency
        //childIsDependent: users[i].childDependency == 'Yes' ? true :
        //  users[i].childDependency == 'No'  ? false : null
      };
      childs.push(child)
    }

      // --- populate partyRole
      const roleData = this.firstFormGroup.value;
      let partyRole: PartyRole = PartyRole.Unknown;
      let partyEnrolled = '';

      if (roleData.firstControl == 'I am the receipent, i currently receive child support')
      {
        partyRole = PartyRole.Recipient;
        partyEnrolled = 'Recipient';
      }
      else
      {
        partyRole = PartyRole.Payor;
        partyEnrolled = 'Payor';
      }

      // --- populate party
      const partyData = this.secondFormGroup.value;
      const file1Data = this.fifthFormGroup.value;
      const file2Data = this.sixFormGroup.value;

      //let LookupValue
      let inGender: LookupValue = { value: partyData.gender };
      let inProvince: LookupValue = { value: partyData.province};
      let inIdentityParty: LookupValue = { value: partyData.identity};
      let inReferral: LookupValue = { value: file2Data.referals };

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
          referral: inReferral
          //preferredContactMethod = { value = inFile2.gender},
          //referenceNumber = null
      }

      // --- populate other party
      const otherPartyData = this.thirdFormGroup.value;
      let inOtherGender: LookupValue = { value: otherPartyData.gender};
      let inOtherProvince: LookupValue = { value: otherPartyData.province};

      let inOtherParty: Party = {
            partyId: "00000000-0000-0000-0000-000000000000",
            firstName: otherPartyData.firstName,
            middleName: otherPartyData.givenNames,
            lastName: otherPartyData.lastName,
            preferredName: otherPartyData.pname,
            dateOfBirth: this.transformDate(otherPartyData.birthdate),
            gender: inOtherGender,
            addressStreet1: otherPartyData.saddress1,
            addressStreet2: otherPartyData.saddress2,
            city: otherPartyData.city,
            province: inOtherProvince,
            postalCode: otherPartyData.postalCode,
            homePhone: otherPartyData.homePhoneNumber,
            workPhone: otherPartyData.workPhoneNumber,
            cellPhone: otherPartyData.cellPhoneNumber,
            email: otherPartyData.email,
            optOutElectronicDocuments: null,
            identity: null,
            referral: null,
            //preferredContactMethod = null,
            //referenceNumber = null
      }

      // --- populate file
      let inCourtFileType: LookupValue = {value: roleData.secondControl};

      //let inBcCourtLocation: CourtLookupValue = {value: file1Data.courtLocation.value};
      let inBcCourtLocation: any;
      if (typeof inBcCourtLocation === 'undefined') {
        inBcCourtLocation = null;
      }
      else
      {
        inBcCourtLocation = file1Data.courtLocation;
      }
      this._logger.info('inBcCourtLocation',inBcCourtLocation);
      this._logger.info('file1Data.courtLocation.value',file1Data.courtLocation.value);

      let inFile:any = {
          status: FileStatus.Unknown,
          usersRole: partyRole,
          fileId: '0',
          fileNumber: null,
          partyEnrolled: partyEnrolled,
          courtFileType: inCourtFileType,
          bcCourtLevel: 'Provincial',
          bcCourtLocation: inBcCourtLocation,
          dateOfOrderOrWA: this.transformDate(file1Data.orderDate),
          incomeOnOrder: file1Data.payorIncome,
          //file2Data.incomeAssistance,
          section7Expenses: file1Data.isSpecifiedIncome,
          safetyAlertRecipient: null,
          recipientSafetyConcernDescription: null,
          safetyAlertPayor: null,
          payorSafetyConcernDescription: null,
          isFMEPFileActive: file2Data.enrollFMEP,
          fmepFileNumber: file2Data.FMEPinput,
          recalculationOrderByCourt: file1Data.recalculationOrdered,
          otherParty: inOtherParty,
          children: childs
      }

      // --- populate
      let newFileRequest: NewFileRequest = {
        bCeiD: '26336072-cba4-4b6e-871b-5355d27df9b3',
        user: inParty,
        file: inFile,
      }

      //if (partyRole == PartyRole.Recipient)
      if (partyEnrolled == 'Recipient')
      {
        newFileRequest.file.safetyAlertRecipient = file2Data.childSafety;
        newFileRequest.file.recipientSafetyConcernDescription = file2Data.childSafetyDescription;
      }
      else
      {
        newFileRequest.file.safetyAlertPayor = file2Data.childSafety;
        newFileRequest.file.payorSafetyConcernDescription = file2Data.childSafetyDescription;
      }

      this._logger.info("newFileRequest:", newFileRequest);

    //this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountCreatePost(newFileRequest).subscribe({
      next: (newFileRequest) => {
        this._reponse = newFileRequest;
        if ( this._reponse.status === HttpStatusCode.Ok) {
          this._logger.info("_reponse:", this._reponse);
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
      complete: () => this._logger.info('apiAccountCreatePost is completed')
    })

    }


}
