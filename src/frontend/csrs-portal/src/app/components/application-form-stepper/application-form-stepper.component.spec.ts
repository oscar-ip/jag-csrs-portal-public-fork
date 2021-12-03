import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationFormStepperComponent } from './application-form-stepper.component';

describe('ApplicationFormStepperComponent', () => {
  let component: ApplicationFormStepperComponent;
  let fixture: ComponentFixture<ApplicationFormStepperComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicationFormStepperComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicationFormStepperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
