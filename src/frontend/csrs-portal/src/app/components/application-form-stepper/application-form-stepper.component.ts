import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
  FormArray,
} from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
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

@Component({
  selector: 'app-application-form-stepper',
  templateUrl: './application-form-stepper.component.html',
  styleUrls: ['./application-form-stepper.component.scss']
})
export class ApplicationFormStepperComponent implements OnInit {

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
  isEditable = false;

  constructor(private _formBuilder: FormBuilder, private http: HttpClient,
    @Inject(AccountService) private accountService,
    @Inject(LookupService) private lookupService,
    @Inject(LoggerService) private logger,
    @Inject(OidcSecurityService) private oidc,
    public dialog: MatDialog) {}

  ngOnInit() {
    this._accountService = this.accountService;
    this._lookupService = this.lookupService;
    this._logger = this.logger;
    this._oidc = this.oidc;

    this.provinceData = [{label: 'province', value: 'British Columbia'}];
    this.identityData = [{label: 'identity', value: 'Native'}];
    this.genderData =  [{label: 'gender', value: 'Male'}];
    this.courtLocationsData =  [{label: 'courtLocation', ssg_bccourtlocationname: 'Victoria Court'}];
    this.referalsData = [{label: 'referal', value: 'FMEP'}];
    this.getReferalsData();
    this.getCourtLocationData();
    this.getIdentityData();
    this.getProvinceData();
    this.getGenderyData();

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
      pname: [],
      birthdate: [],
      saddress1: [],
      saddress2: [],
      city: [''],
      province: [],
      postalCode: [],
      homePhoneNumber: [],
      cellPhoneNumber: [],
      workPhoneNumber: [],
      email: ['', Validators.required],
      gender: []

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
      firstName: [''],
      orderDate: [],
      courtLocation: []
    });
    this.sixFormGroup = this._formBuilder.group({
      // secondCtrl: ['', Validators.required],
      childSafety: [''],
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

    this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
      this._accountService.apiAccountIdentitiesGet().subscribe({
        next: (data) => this.identityData = data,

        error: (e) => {
          if (e.error instanceof Error) {
            this._logger.error(e.error.message);
          } else {
              //Backend returns unsuccessful response codes such as 404, 500 etc.
              this._logger.info('Backend returned ', e);
            }
        },
        complete: () => this._logger.info('apiAccountIdentitiesGet is completed')
    })
  }

  getProvinceData() {
    this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountProvincesGet().subscribe({
      next: (data) => this.provinceData = data,
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
    this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountGendersGet().subscribe({
      next: (data) => this.genderData = data,
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
    this._lookupService.configuration.accessToken =  this._oidc.getAccessToken();
      this._lookupService.apiLookupCourtlocationsGet().subscribe({
        next: (data) => this.courtLocationsData = data,
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
    this._accountService.configuration.accessToken =  this._oidc.getAccessToken();
    this._accountService.apiAccountReferralsGet().subscribe({
      next: (data) => this.referalsData = data,
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
    localStorage.setItem('formData', JSON.stringify(formData));
  }
  save(){
    localStorage.getsetItemItem('formData', '');
  }
}
