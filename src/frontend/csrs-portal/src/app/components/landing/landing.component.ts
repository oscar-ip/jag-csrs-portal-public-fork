import { AfterViewInit, Component } from '@angular/core';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss'],
})
export class LandingComponent implements AfterViewInit {
  public bceIdLink: string;
  
  constructor() {
    this.bceIdLink = "https://www.bceid.ca/";
  }

  public ngAfterViewInit(): void {
  }
}