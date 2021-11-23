import { AfterViewInit, Component, ViewEncapsulation  } from '@angular/core';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LandingComponent implements AfterViewInit {
  public bceIdLink: string;
  public cscLink: string;
  
  constructor() {
    this.bceIdLink = "https://www.bceid.ca/";
    this.cscLink = "https://www.childsupportcalculator.ca/british-columbia.html";
  }

  public ngAfterViewInit(): void {
  }
}