import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChildApplicationQuestionComponent } from './child-application-question.component';

describe('ChildApplicationQuestionComponent', () => {
  let component: ChildApplicationQuestionComponent;
  let fixture: ComponentFixture<ChildApplicationQuestionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChildApplicationQuestionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChildApplicationQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
