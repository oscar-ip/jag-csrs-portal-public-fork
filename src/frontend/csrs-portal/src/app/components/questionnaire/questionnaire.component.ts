import { style } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { LoggerService } from '@core/services/logger.service';
import { Router, ActivatedRoute } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Inject } from '@angular/core';
import { AppConfigService } from 'app/services/app-config.service';

@Component({
  selector: 'app-questionnaire',
  templateUrl: './questionnaire.component.html',
  styleUrls: ['./questionnaire.component.scss'],
})

export class QuestionnaireComponent implements OnInit {
  public isLoggedIn = false;
  public bceIdRegisterLink: string;
  public _logger: LoggerService;
  public _oidc: OidcSecurityService;
  public _router: Router;
  public   isEditable = true;
  public welcomeUser: string;
  public _config: AppConfigService;


  data: any = [
    {
      label:
      'Do you have an order made in the Provincial Court of British Columbia or a written agreement that is filed in the Provincial Court of British Columbia that deals with child support payments?',
      content: [],
      buttons: [
        {
          label: 'Yes',
          clickedContent: [
            {
              id: 'content-1',
              data: '<span class="text-dark"><strong>Great!</strong> You’ll need to provide information from your order or written agreement if you enrol in the Child Support Recalculation Service.</span>',
            },
            {
              data: ' <span class="text-dark">To apply for the recalculation service, you must have an order or written agreement for child support from the Provincial Court of British Columbia. You\'ll need a copy of it before you start your application.</span>',
            },
             {data: ' <span class="text-dark">If you have an order or written agreement but do not have a copy, ask <a href=\'https://www.provincialcourt.bc.ca/locations-contacts\' target="_blank"><span style="color:#009cde !important; text-decoration: underline">the court</span></a> for one. </span>'},
            {
              data: ' <span class="text-dark">You’ll need to have a copy of it before you start your application.</span>',
            },
            {
              data: ' <span class="text-dark">The Child Support Recalculation Service does <strong>not</strong> recalculate:</span>',
            },
            {
              data: {
                ul: [
                  {
                    liData:
                      ' <span class="text-dark">orders and written agreements for spousal support; and</span>',
                  },
                  {
                    liData:
                      ' <span class="text-dark">orders and written agreements for child support from the Supreme Court of British Columbia.</span>',
                  },
                ],
              },
            },
          ],
        },
        {
          label: 'No',
          clickedContent: [
            {
              data: ' <span class="text-dark">To apply for the Child Support Recalculation Service, you must have an order or written agreement for child support from the Provincial Court of British Columbia. You\'ll need a copy of it before you start your application.',
            },
            {
              data: '<span class="text-dark">If you need help with getting a new order or written agreement for child support, see these resources:',
            },
            {
              data: {
                ul: [
                  {
                    liData:
                      '<a style="color:#009cde !important; text-decoration: underline" href="https://www2.gov.bc.ca/gov/content/life-events/divorce/family-justice" target="_blank">British Columbia Family Justice</a>',
                  },
                  {
                    liData:
                      '<a style="color:#009cde !important; text-decoration: underline" href="https://www2.gov.bc.ca/gov/content/life-events/divorce/family-justice/who-can-help/family-justice-centres" target="_blank">Family Justice Centres</a>',
                  },
                  {
                    liData:
                      '<a style="color:#009cde !important; text-decoration: underline" href="https://family.legalaid.bc.ca/" target="_blank">Legal Aid BC - Family Law</a>',
                  },
                ],
              },
            },
            {
              data: '<span class="text-dark">If you already have an order or written agreement but do not have a copy, ask <a href=\'https://www.provincialcourt.bc.ca/locations-contacts\' target="_blank"><span style="color:#009cde !important; text-decoration: underline">the court</span></a> for one. ',
            },
            {
              data: '<span class="text-dark">The Child Support Recalculation Service does <strong>not</strong> recalculate:',
            },
            {
              data: '<li><span class="text-dark">orders and written agreements for spousal support; and</span></li>',
            },
            {
              data: '<li><span class="text-dark">orders and written agreements for child support from the Supreme Court of British Columbia.</span></li>',
            },
          ],
        },
      ],
      forwardButtons: [
        {
          label: 'Next',
          name: 'next',
        },
      ],
    },
    {
      label: 'Do you, the child or children, and the other parent all live in British Columbia?',

      content: [],
      buttons: [
        {
          label: 'Yes',
          clickedContent: [],
        },
        {
          label: 'No',
          clickedContent: [
            {
              data: '<span class="text-dark">To use the Child Support Recalculation Service, both you and the other parent or guardian must be B.C. residents. You’re considered a B.C. resident if you have an address in the province and spend the majority of your time in the province.',
            },
            {
              data: '<span class="text-dark">If you or the other parent or guardian live in another province or territory, this service is not available to you.',
            },
            {
              data: '<span class="text-dark">To learn about other options that might be available to you, see these resources:',
            },
            {
              data: {
                ul: [
                  {
                    liData:
                      '<a href="https://www.isoforms.bc.ca/" target="_blank" style="color:#009cde !important; text-decoration: underline">British Columbia Interjurisdictional Support Orders Forms</a>',
                  },
                  {
                    liData:
                      '<a href="https://www2.gov.bc.ca/gov/content/life-events/divorce/family-justice/who-can-help/inter-jurisdictional-support-orders" target="_blank" style="color:#009cde !important; text-decoration: underline">British Columbia’s Inter-Jurisdictional Support Orders</a>',
                  },
                  {
                    liData:
                      '<a href="https://www2.gov.bc.ca/gov/content/life-events/divorce/family-justice" target="_blank" style="color:#009cde !important; text-decoration: underline">British Columbia Family Justice</a>',
                  },
                ],
              },
            },
          ],
        },
      ],
      forwardButtons: [
        {
          label: 'Back',
          name: 'back',
        },
        {
          label: 'Next',
          name: 'next',
        },
      ],
    },
    {
      label: 'Has the financial situation of you or the other parent or guardian changed since your support order or written agreement was made?',
      content: [],
      buttons: [
        {
          label: 'Yes',
          clickedContent: [
            {
              data: '<span class="text-dark">In B.C., the amount of child support to be paid is determined using <a href="https://www2.gov.bc.ca/gov/content/life-events/divorce/family-justice/family-law/child-support/guidelines" target="_blank" style="color:#009cde !important; text-decoration: underline">the Child Support Guidelines</a>. As a general rule, the law requires courts, parents and the Child Support Recalculation Service to use the guidelines.',
            },
            {
              data: '<span class="text-dark">Even if you\'re not sure if the income of the other parent has changed, the recalculation service can review your order or written agreement each year to ensure it is fair based on the most recent income tax information and the Child Support Guidelines.',
            },
            {
              data: '<span class="text-dark">Each year, the recalculation service will ask the paying parent to provide their income tax information for the most recent tax year.',
            },
            {
              data: '<span class="text-dark">In some situations, the recalculation service will ask both parents to provide this information if their child support order or written agreement used both incomes to determine support.',
            },
            {
              data: '<span class="text-dark">If income tax information is not provided by the time it is required, the recalculation service can add at least 10% or more to the income amount set out in the most recent order or recalculation statement and then recalculate the amount of child support based on the increased income amount.  The rate of increase will depend on the date the most recent order was made or last recalculated when income was reviewed.',
            },
          ],
        },
        {
          label: 'No',
          clickedContent: [
            {
              data: '<span class="text-dark">In B.C., the amount of child support to be paid is determined using <a href="https://www2.gov.bc.ca/gov/content/life-events/divorce/family-justice/family-law/child-support/guidelines" target="_blank" style="color:#009cde !important; text-decoration: underline">the Child Support Guidelines</a>. As a general rule, the law requires courts, parents and the Child Support Recalculation Service to use the guidelines.',
            },
            {
              data: '<span class="text-dark">Even if you\'re not sure if the income of the other parent has changed, the recalculation service can review your order or written agreement each year to ensure it is fair based on the most recent ncome tax information and the Child Support Guidelines.',
            },
            {
              data: '<span class="text-dark">Each year, the recalculation service will ask the paying parent to provide their income tax information for the most recent tax year.',
            },
            {
              data: '<span class="text-dark">In some situations, the recalculation service will ask both parents to provide this information if their child support order or written agreement used both incomes to determine support.',
            },
            {
              data: '<span class="text-dark">If income tax information is not provided by the time it is required, the recalculation service can add at least 10% or more to the income amount set out in the most recent order or recalculation statement and then recalculate the amount of child support based on the increased income amount.  The rate of increase will depend on the date the most recent order was made or last recalculated when income was reviewed.',
            },
          ],
        },
        {
          label: 'I don’t know',
          clickedContent: [
            {
              data: '<span class="text-dark">In B.C., the amount of child support to be paid is determined using <a href="https://www2.gov.bc.ca/gov/content/life-events/divorce/family-justice/family-law/child-support/guidelines" target="_blank" style="color:#009cde !important; text-decoration: underline">the Child Support Guidelines</a>.As a general rule, the law requires courts, parents and the Child Support Recalculation Service to use the guidelines.',
            },
            {
              data: '<span class="text-dark">Even if you\'re not sure if the income of the other parent has changed, the recalculation service can review your order or written agreement each year to ensure it is fair based on the most recent ncome tax information and the Child Support Guidelines.',
            },
            {
              data: '<span class="text-dark">Each year, the recalculation service will ask the paying parent to provide their income tax information for the most recent tax year.',
            },
            {
              data: '<span class="text-dark">In some situations, the recalculation service will ask both parents to provide this information if their child support order or written agreement used both incomes to determine support.',
            },
            {
              data: '<span class="text-dark">If income tax information is not provided by the time it is required, the recalculation service can add at least 10% or more to the income amount set out in the most recent order or recalculation statement and then recalculate the amount of child support based on the increased income amount.  The rate of increase will depend on the date the most recent order was made or last recalculated when income was reviewed.',
            },
          ],
        },
      ],
      forwardButtons: [
        {
          label: 'Back',
          name: 'back',
        },
        {
          label: 'Next',
          name: 'next',
        },
      ],
    },
    {
      label: 'Was the amount of child support in your order or written agreement based on any of the following situations?',

      content: [
        {
          label:
            '',
        },
        {
          data: {
            ol: [
              {
                liData: '<strong>Undue hardship;</strong>',
                tooltipData: 'The court determines the child support amount to be lower or higher than the child support guidelines table amount because of special circumstances that caused exceptional difficulty for the person paying.'
              },
              {
                liData: '<strong>Self-employment or partnership income; or</strong>',
                tooltipData: 'The court determines the child support amount to be lower or higher than the child support guidelines table amount because of special circumstances that caused exceptional difficulty for the person paying.'
              },
              {
                liData: '<strong>Payor stands in place of a parent.</strong>',
                tooltipData: 'The court determines the child support amount to be lower or higher than the child support guidelines table amount because of special circumstances that caused exceptional difficulty for the person paying.'
              },
              {
                liData: '<strong>Pattern of income</strong>',
                tooltipData: 'The court determines the child support amount to be lower or higher than the child support guidelines table amount because of special circumstances that caused exceptional difficulty for the person paying.'
              },
            ],
          },
        },
      ],
      buttons: [
        {
          label: 'Yes',
          clickedContent: [
            {
              data: '<span class="text-dark">The Child Support Recalculation Service can only recalculate support based on the Child Support Guidelines. Unlike a judge, it <strong>cannot</strong> consider any other factors in determining the support payable.',
            },
            {
              data: '<span class="text-dark">If your child support was based on any of the above, the recalculation service <strong>cannot</strong> recalculate it and you are not eligible for the service.',
            },
          ],
        },
        {
          label: 'No',
          clickedContent: [
            {
              data: '<span class="text-dark">The Child Support Recalculation Service can only recalculate support based on the Child Support Guidelines. Unlike a judge, it <strong>cannot</strong> consider any other factors in determining the support payable.',
            },
            {
              data: '<span class="text-dark">If your child support was based on any of the above, the recalculation service <strong>cannot</strong> recalculate it and you are not eligible for the service.',
            },
          ],
        },
        {
          label: 'I don’t know',
          clickedContent: [],
        },
      ],
      forwardButtons: [
        {
          label: 'Back',
          name: 'back',
        },
        {
          label: 'Next',
          name: 'next',
        },
      ],
    },
    {
      label: 'Was the amount of child support in your order based on imputed income ?',
      content: [],
      buttons: [
        {
          label: 'Yes',
          clickedContent: [
            {
              data: '<span class="text-dark">The Child Support Recalculation Service <strong>cannot</strong> recalculate support when a paying parent’s incomehas been imputed. This is because the recalculation service cannot make decisions that a judge can make about why income was imputed when child support was determined.If income has been imputed, your court order will usually say so.',
            },
            {
              data: '<span class="text-dark"><strong>There is one exception:</strong> If the order states that the paying parent’s income was imputed because they are exempt from paying federal or provincial income tax, then the recalculation service may be able to recalculate.',
            },
          ],
        },
        {
          label: 'No',
          clickedContent: [],
        },
        {
          label: 'I don’t know',
          clickedContent: [],
        },
      ],
      forwardButtons: [
        {
          label: 'Back',
          name: 'back',
        },
        {
          label: 'Next',
          name: 'next',
        },
      ],
    },
    {
      label: ' Does your order or written agreement require the payment of special or extraordinary expenses ?',
      content: [],
      buttons: [
        {
          label: 'Yes',
          clickedContent: [
            {
              data: '<span class="text-dark">The Child Support Recalculation Service may only recalculate special or extraordinary expenses if:',
            },
            {
              data: {
                ul: [
                  {
                    liData:
                      '<span class="text-dark">Both parent’s incomes are stated in the order or written agreement; and',
                  },
                  {
                    liData:
                      '<span class="text-dark">Each parent’s proportion owing in relation to their incomes is clearly set out',
                  },
                ],
              },
            },
            {
              data: '<span class="text-dark">The recalculation service must review your order or written agreement first to determine whether it can recalculate your special or extraordinary Section 7 expenses.',
            },
            {
              data: '<span class="text-dark">The recalculation service doesn’t have the authority to determine whether an expense continues to be eligible and payable. It may only adjust each parent’s proportionate share owing based on updated incomes.',
            },
            {
              data: '<span class="text-dark">If parents cannot agree on the eligibility of expenses, they may ask a court to decide.',
            },
          ],
        },
        {
          label: 'No',
          clickedContent: [],
        }
      ],
      forwardButtons: [
        {
          label: 'Back',
          name: 'back',
        },
        {
          label: 'Next',
          name: 'next',
        },
      ],
    },
    {
      label:
        'Does your order/written agreement include child support involving',
      content: [
        {
          label:
            '',
        },
        {
          data: {
            ol: [
              {
                liData: '<strong>Children at or over the age of 19 ;</strong>',
                tooltipData: 'The court determines the child support amount to be lower or higher than the child support guidelines table amount because of special circumstances that caused exceptional difficulty for the person paying.',
              },
              {
                liData: '<strong>Shared parenting ;</strong>',
                tooltipData: 'The court determines the child support amount',

              },
              {
                liData: '<strong>Income over $150,000 ?</strong>',
                tooltipData: 'The court determines the child support amount',

              },
            ],
          },
        },
      ],
      buttons: [
        {
          label: 'Yes',
          clickedContent: [
            {
              data: '<span class="text-dark">These areas of child support can be complex. Sometimes a judge may apply factors other than the Child Support Guideline Tables to determine the amount of child support. The Child Support Recalculation Service must review your order or written agreement first to decide whether it can recalculate your child support amount.',
            },
          ],
        },
        {
          label: 'No',
          clickedContent: [],
        },
        {
          label: 'I don’t know',
          clickedContent: [],
        },
      ],
      forwardButtons: [
        {
          label: 'Back',
          name: 'back',
        },
        {
          label: 'Next',
          name: 'next',
        },
      ],
    },
    {
      label:
      ' Are there any court applications underway between you and the other parent that could have an impact on the amount of child support in your order or written agreement?',
      content: [
        {
          label:
            'For example, a party is applying to reduce or cancel child support, or someone is applying to change the parenting arrangements where a child will spend a different amount of time with each parent.',
        },
      ],
      buttons: [
        {
          label: 'Yes',
          clickedContent: [
            {
              data: '<span class="text-dark">If you or the other parent have legal proceedings underway that may impact child support payments, the recalculation service <strong>cannot</strong> recalculate your order. Once you obtain a new order, you can apply to the Child Support Recalculation Service.',
            },
            {
              data: '<span class="text-dark">The Child Support Recalculation Service recalculates child support in a less informal way than court. Parents enrolled in the service won\'t have to go to court and ask a judge to decide on child support when their income changes.',
            },
            {
              data: '<span class="text-dark"><strong>Important note:</strong> The Child Support Recalculation Service cannot backdate the recalculation of your child support. It can only recalculate child support amounts going forward based on income tax information for the most recent tax year.',
            },
            {
              data: '<span class="text-dark">The recalculation service provides a copy of the recalculation statement to both parents and files a copy in court. Once it\'s in effect, the recalculated amount is the child support to be paid.',
            },
          ],
        },
        {
          label: 'No',
          clickedContent: [],
        },
      ],
      forwardButtons: [
        {
          label: 'Back',
          name: 'back',
        },
        {
          label: 'Next',
          name: 'next',
        },
      ],
    },
  ];

  constructor(@Inject(LoggerService) private logger,
              @Inject(Router) private router,
              @Inject(ActivatedRoute) private route,
              @Inject(OidcSecurityService) private oidcSecurityService,
              @Inject(AppConfigService) private appConfigService) {

          this._config = appConfigService;
          this._logger = logger;
          this._logger.log('info', 'Questionnaire: constructor');

          const accessToken = oidcSecurityService.getAccessToken();

          if (accessToken) {
            this._logger.log('info', `Questionnaire: accessToken: ${accessToken}`);
          }

          if (oidcSecurityService.isAuthenticated()) {
            this._logger.log('info', 'Questionnaire: authenticated');
          } else {
            this._logger.log('info', 'Questionnaire: not authenticated');
          }
  }

  stringToHTML(i, yi, ci, str, idLabel) {
    const style = 'style';
    const parser = new DOMParser();
    const doc = parser.parseFromString(str, 'text/html');
    const id = '#' + idLabel + '-' + i + '-' + yi + '-' + ci;
    if (
      document.querySelector(id) &&
      document.querySelector(id).childElementCount === 0
    ) {
      document.querySelector(id).appendChild(doc.body);
      
    }
    document.querySelector(id).setAttribute('color', 'color:#2E8540 !important');

    doc.body[style].cssText += 'color:#2E8540 !important';
  }
  setLabel(question: any, buttonItem: any, index: any) {
    question.clicked = buttonItem.label;
    question.isYes = buttonItem.label === 'No' ? false : true;
  }
  setUIconColor(index, question) {

    question.submit = true;
    const style = 'style';
    setTimeout(() => {
      const node: Node = document.querySelector(
        '#cdk-step-label-0-' + index + ' .mat-step-icon-state-edit'
      );
      if (question.clicked === 'Yes' && node && [4,5,8].includes(index+1))  {
        node[style].cssText += 'background-color:#D8292F !important';
      } else if (node && (question.clicked === 'Yes' || question.clicked === 'I don’t know')) {
        node[style].cssText += 'background-color:#2E8540 !important';
      } else if (node && question.clicked === 'No' && [3,4,5,6,7,8].includes(index+1)) {
        node[style].cssText += 'background-color:#2E8540 !important';
      } else if (node && question.clicked === 'No') {
        node[style].cssText += 'background-color:#D8292F !important';
      } 
    });
  }

  setUIconColor1(index, question) {
      if (question.clicked === 'Yes' && [4,5,8].includes(index+1))  {
        return 'clear'
      } else if ((question.clicked === 'Yes' || question.clicked === 'I don’t know')) {
        return 'done'
      } else if (question.clicked === 'No' && [3,4,5,6,7,8].includes(index+1)) {
        return 'done'
      } else if (question.clicked === 'No') {
        return 'clear'
      }
      return 'edit' 
    }

  setColor(question,buttonItem,i){
    let myGreen = false;
    let myRed = false;
    if((question.clicked == buttonItem.label &&  question.clicked == 'Yes')){
    
      if([4,5,8].includes(i+1)){
        myRed = true
      } else {
        myGreen = true
      }
    }
    if((question.clicked == buttonItem.label &&  question.clicked == 'No')){
     
      if([3,4,5,6,7,8].includes(i+1)){
        myGreen = true
      } else {
        myRed = true
      }

    }
    if((question.clicked == buttonItem.label &&  question.clicked == 'I don’t know')){
      myGreen = true
    }

    return { 'myGreen' : myGreen, 'myRed' : myRed  }
  }

  setColorIcon(question,buttonItem,i){
    let myGreen = false;
    let myRed = false;
    if ((question.clicked == buttonItem.label &&  question.clicked == 'Yes')){
   
     if([4,5,8].includes(i+1)){
       myRed = true;
     } else {
       myGreen = true;
     }
     this.setUIconColor(i, question);
   }
    if ((question.clicked === buttonItem.label &&  question.clicked == 'No')){
    
     if ([3, 4 , 5 , 6 , 7 , 8].includes(i + 1)){
       myGreen = true
     } else {
       myRed = true
     }
     this.setUIconColor(i,question);

   }
    if ((question.clicked == buttonItem.label &&  question.clicked === 'I don’t know')){
     myGreen = true
   }

    // tslint:disable-next-line: object-literal-key-quotes
    return { 'myGreen' : myGreen, 'myRed' : myRed  };
 }
  public async ngOnInit() {

    this.bceIdRegisterLink = this._config.bceIdRegisterLink;
    this.oidcSecurityService.checkAuth().subscribe(({ isAuthenticated, userData, accessToken, idToken }) => {

      this._logger.log('info',`Questionnaire: isAuthenticated = ${isAuthenticated}`);
      this._logger.log('info',`Questionnaire: userData = ${userData}`);
      this._logger.log('info',`Questionnaire: accessToken = ${accessToken}`);
      this._logger.log('info',`Questionnaire: idToken = ${idToken}`);

      if (isAuthenticated === true)
      {
        this.router.navigate(['/welcomeuser']);
      }
    });
  }

  login() {
    this._logger.log('info','Questionnaire: inside login');
    this.oidcSecurityService.authorize();
  }

  logout() {
    this._logger.log('info','Questionnaire: inside logout');
    this.oidcSecurityService.logoff();
  }

  downloadApplication()
  {
    const link = document.createElement('a');
    link.download = "Application.pdf";
    link.href = "assets/Application.pdf";
    link.click();
  }

}
