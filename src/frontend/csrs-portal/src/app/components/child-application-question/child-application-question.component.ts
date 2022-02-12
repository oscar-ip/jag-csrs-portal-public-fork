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

  provinces: any = [];
  genders: any = [];
  identities: any = [];
  referrals: any = [];
  preferredContactMethods: any = [];
  courtLevels: any = [];
  courtLocations: any = [];

  _accountService: AccountService;
  _lookupService: LookupService;
  _logger: LoggerService;
  _oidc: OidcSecurityService;
  today = new Date();
  isEditable: boolean = false;
  child: Child;
  _reponse: HttpResponse<any>;

  _yes: number = 867670000;
  _no: number = 867670001;
  _iDontKnow: number = 867670002;

  _courtOrder: number = 867670000;
  _writtenAgreement: number = 867670001;

  data: any = null;
  result: any = [];

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

    this.provinces = [{id: '123', value: 'British Columbia'}];
    this.identities = [{id: '123', value: 'Native'}];
    this.genders =  [{id: '123', value: 'Male'}];
    this.courtLocations =  [{id: '123', value: 'Victoria Court'}];
    this.referrals = [{id: '123', value: 'FMEP'}];

    this.getReferrals();
    this.getIdentities();
    this.getProvinces();
    this.getGenders();
    this.getPreferredcontactmethods();
    this.getCourtLevels();
    this.getCourtLocations();


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
      referral: [''],

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

openDialog(inData) {
  const dialogRef = this.dialog.open(ConfirmDialogComponent,{
    width: '550px',
    data: inData
  });

  dialogRef.afterClosed().subscribe(result => {
    console.log(`Dialog result: ${result}`);
  });
}

editPage(stepper, index){
  this.isEditable = true;
  stepper.selectedIndex = index;
}
  getIdentities() {
    //this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountIdentitiesGet().subscribe({
        next: (data) => {
          this.identities = data;
          this._logger.info('this.identities',this.identities);
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

  getProvinces() {
    //this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountProvincesGet().subscribe({
      next: (data) => {
        this.provinces = data;
        this._logger.info('this.provinces',this.provinces);
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

  getGenders() {
    //this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountGendersGet().subscribe({
      next: (data) => {
        this.genders = data;
        this._logger.info('this.genders',this.genders);
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

  getCourtLocations() {
    //this._lookupService.configuration.accessToken =  this._oidc.getAccessToken();
      this._lookupService.apiLookupCourtlocationsGet().subscribe({
        next: (data) => {
          this.courtLocations = data;
          this._logger.info('this.courtLocations',this.courtLocations);
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

  getCourtLevels() {
    //this._lookupService.configuration.accessToken =  this._oidc.getAccessToken();
      this._lookupService.apiLookupCourtlevelsGet().subscribe({
        next: (data) => {
          this.courtLevels = data;
          this._logger.info('this.courtLevels',this.courtLevels);
        },
        error: (e) => {
          if (e.error instanceof Error) {
            this._logger.error(e.error.message);
          } else {
              //Backend returns unsuccessful response codes such as 404, 500 etc.
              this._logger.info('Backend returned ', e);
            }
        },
        complete: () => this._logger.info('courtlevelsGet is completed')
      })
  }


  getReferrals() {
    //this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountReferralsGet().subscribe({
      next: (data) => {
        this.referrals = data;
        this._logger.info('this.referals',this.referrals);
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

  getPreferredcontactmethods(){
    //this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountPreferredcontactmethodsGet().subscribe({
      next: (data) => {
        this.preferredContactMethods = data;
        this._logger.info('this.preferredContactMethods',this.preferredContactMethods);
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

  getCourtTyleFile(value){
    const courtTypeFileId = value == 'Court Order' ?  this._courtOrder : this._writtenAgreement;
    const inCourtFileType: LookupValue = {id: courtTypeFileId, value: value};
    return inCourtFileType;
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

      // --- populate other party
      const otherPartyData = this.thirdFormGroup.value;
      let inOtherGender: LookupValue = listGender.firstOrDefault(x=>x.id == otherPartyData.gender);
      let inOtherProvince: LookupValue = listProvince.firstOrDefault(x=>x.id == otherPartyData.province);

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
            preferredContactMethod: null,
            referenceNumber: null
      }

      // --- populate file
      let inCourtFileType: LookupValue = this.getCourtTyleFile(roleData.secondControl);

      let listBcCourtLocation = new List<LookupValue>(this.courtLocations);
      let inBcCourtLocation: LookupValue = listBcCourtLocation.firstOrDefault(x=>x.id == file1Data.courtLocation);

      //let inBcCourtLevel: CourtLookupValue = this.courtLevels[0];
      let listBcCourtLevel = new List<LookupValue>(this.courtLevels);
      let inBcCourtLevel: LookupValue = listBcCourtLevel.firstOrDefault(x=>x.value == 'Provincial');

      let inFile:any = {
          status: FileStatus.Unknown,
          usersRole: partyRole,
          fileId: '0',
          fileNumber: null,
          partyEnrolled: partyEnrolled,
          courtFileType: inCourtFileType,
          bcCourtLevel: inBcCourtLevel,
          bcCourtLocation: inBcCourtLocation,
          dateOfOrderOrWA: this.transformDate(file1Data.orderDate),
          incomeOnOrder: file1Data.payorIncome,
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
        user: inParty,
        file: inFile,
      }

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
      next: (outData:any) => {

        var partyId = outData.partyId;
        var fileId = outData.fileId;
        var fileNumber = outData.fileNumber;

        this._logger.info("partyId", partyId);
        this._logger.info("fileId", fileId);
        this._logger.info("fileNumber", fileNumber);

        let customOptions: DialogOptions = { data: {fileNumber: fileNumber}};
        this.openDialog(customOptions);
      },
      error: (e) => {
        if (e.error instanceof Error) {
          this._logger.error(e.error.message);

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
            this._logger.info('Backend returned ', e);
          }
      },
      complete: () => this._logger.info('apiAccountCreatePost is completed')
    })

    }


}
