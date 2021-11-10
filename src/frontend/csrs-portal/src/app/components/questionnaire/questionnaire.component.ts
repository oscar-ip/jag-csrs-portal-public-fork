import { AfterViewInit, Component } from '@angular/core';

@Component({
  selector: 'app-questionnaire',
  templateUrl: './questionnaire.component.html',
  styleUrls: ['./questionnaire.component.scss']
})
export class QuestionnaireComponent implements AfterViewInit {

  public bceIdLink: string;
  //Question 1
  a1Yes: boolean = false;
  a1No: boolean = false;
  //Question 2
  a2Yes: boolean = false;
  a2No: boolean = false;
  //Question 3
  a3Yes: boolean = false;
  a3No: boolean = false;
  a3DontKnow: boolean = false;
  //Question 4
  a4Yes: boolean = false;
  a4No: boolean = false;
  a4DontKnow: boolean = false;
  //Question 5
  a5Yes: boolean = false;
  a5No: boolean = false;
  a5DontKnow: boolean = false;
  //Question 6
  a6Yes: boolean = false;
  a6No: boolean = false;
  //Question 7
  a7Yes: boolean = false;
  a7No: boolean = false;
  //Question 8
  a8Yes: boolean = false;
  a8No: boolean = false;

  constructor() {
    this.bceIdLink = "https://www.bceid.ca/";
   }

  public ngAfterViewInit(): void {
  }

  // Question 1
  q1Yes()
  {
    this.a1Yes = true; this.a1No = !this.a1Yes;
  }
  q1No()
  {
    this.a1No = true; this.a1Yes = !this.a1No;
  }

  // Question 2
  q2Yes()
  {
    this.a2Yes = true; this.a2No = !this.a2Yes;
  }
  q2No()
  {
    this.a2No = true; this.a2Yes = !this.a2No;
  }

  // Question 3
  q3Yes()
  {
    this.a3Yes = true; this.a3No = !this.a3Yes;this.a3DontKnow = !this.a3Yes;
  }
  q3No()
  {
    this.a3No = true; this.a3Yes = !this.a3No;this.a3DontKnow = !this.a3No;
  }
  q3DontKnow()
  {
    this.a3DontKnow = true; this.a3Yes = !this.a3DontKnow;this.a3No = !this.a3DontKnow;
  }

  // Question 4
  q4Yes()
  {
    this.a4Yes = true; this.a4No = !this.a4Yes;this.a4DontKnow = !this.a4Yes;
  }
  q4No()
  {
    this.a4No = true; this.a4Yes = !this.a4No;this.a4DontKnow = !this.a4No;
  }
  q4DontKnow()
  {
    this.a4DontKnow = true; this.a4Yes = !this.a4DontKnow;this.a4No = !this.a4DontKnow;
  }
  // Question 5
  q5Yes()
  {
    this.a5Yes = true; this.a5No = !this.a5Yes;this.a5DontKnow = !this.a5Yes;
  }
  q5No()
  {
    this.a5No = true; this.a5Yes = !this.a5No;this.a5DontKnow = !this.a5No;
  }
  q5DontKnow()
  {
    this.a5DontKnow = true; this.a5Yes = !this.a5DontKnow;this.a5No = !this.a5DontKnow;
  }
  // Question 6
  q6Yes()
  {
    this.a6Yes = true; this.a6No = !this.a6Yes;
  }
  q6No()
  {
    this.a6No = true; this.a6Yes = !this.a6No;
  }
  // Question 7
  q7Yes()
  {
    this.a7Yes = true; this.a7No = !this.a7Yes;
  }
  q7No()
  {
    this.a7No = true; this.a7Yes = !this.a7No;
  }
  // Question 8
  q8Yes()
  {
    this.a8Yes = true; this.a8No = !this.a8Yes;
  }
  q8No()
  {
    this.a8No = true; this.a8Yes = !this.a8No;
  }

}
