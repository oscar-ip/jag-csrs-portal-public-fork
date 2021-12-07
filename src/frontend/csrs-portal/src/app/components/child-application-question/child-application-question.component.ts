import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
  FormArray,
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

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
  constructor(private _formBuilder: FormBuilder, private http: HttpClient) {}

  ngOnInit() {
    this.provinceData = [{label: 'hai', value: 1234}];
    this.identityData = [{label: 'hai', value: 1234}];
    this.genderData =  [{label: 'hai', value: 1234}];
    this.getIdentityData();
    this.getProvinceData();
    this.getGenderyData();
    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ['', Validators.required],
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
      lastName: ['', Validators.required],
      pname: [],
      birthdate: [],
      givenNames: [],
      saddress: [],
      sadress1: [],
      province: [],
      postalCode: [],
      phoneNumber: []
    });
    
    this.fourthFormGroup1 = this._formBuilder.group({
      users: this._formBuilder.array([
        this._formBuilder.group({
          firstName: [''],
          lastName: [''],
          birthdate: [],
          givenNames: [],
          middleName: []
        })
       
      ])
    });
    this.fourthFormGroup = this._formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      birthdate: [],
      givenNames: [],
      middleName: []
    });
    this.fifthFormGroup = this._formBuilder.group({
      firstName: [''],
      birthdate: [],
    });
    this.sixFormGroup = this._formBuilder.group({
      // secondCtrl: ['', Validators.required],
    });
    this.seventhFormGroup = this._formBuilder.group({
      secondCtrl: [''],
    });
    this.eFormGroup = this._formBuilder.group({
      secondCtrl: [''],
    });
    this.nineFormGroup = this._formBuilder.group({
      secondCtrl: [''],
    });
  }

  getIdentityData() {
    this.http.get('https://ed92ce0e-25ce-4a67-a586-e53fc4379ab9.mock.pstmn.io/identity').subscribe(data => {
      console.log('data', data);
      this.identityData = [{label: 'hai', value: 1234}];
    });
  }
  getProvinceData() {
    this.http.get('https://ed92ce0e-25ce-4a67-a586-e53fc4379ab9.mock.pstmn.io/province').subscribe(data1 => {
      console.log('data', data1);
      // this.provinceData = data1;
      this.provinceData = [{label: 'hai', value: 1234}];
    });
  }
  getGenderyData() {
    this.http.get('https://ed92ce0e-25ce-4a67-a586-e53fc4379ab9.mock.pstmn.io/gender').subscribe(data2 => {
      console.log('data', data2);
      this.genderData =  [{label: 'hai', value: 1234}];
    });
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

}
