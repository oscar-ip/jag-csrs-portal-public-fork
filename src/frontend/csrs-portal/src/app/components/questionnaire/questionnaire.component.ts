import { style } from '@angular/animations';
import { Component, OnInit, ViewChild } from '@angular/core';
import { LoggerService } from '@core/services/logger.service';
import { Router, ActivatedRoute } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Inject } from '@angular/core';
import { AppConfigService } from 'app/services/app-config.service';
import { MatStepper } from '@angular/material/stepper';

@Component({
  selector: 'app-questionnaire',
  templateUrl: './questionnaire.component.html',
  styleUrls: ['./questionnaire.component.scss'],
})

export class QuestionnaireComponent implements OnInit {

  @ViewChild('stepper') stepper: MatStepper;

  public isLoggedIn = false;
  public bceIdRegisterLink: string;
  public   isEditable = true;
  public welcomeUser: string;
  public selectedIndex: number = 0;


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
              data: '<span class="text-dark"><strong>Great!</strong></span>',
            },
            {
              data: ' <span class="text-dark">To apply for the recalculation service, you must have an order or written agreement for child support from the Provincial Court of British Columbia. You\'ll need a copy of it before you start your application.</span>',
            },
             {data: ' <span class="text-dark">If you have an order or written agreement but do not have a copy, ask <a href=\'https://www.provincialcourt.bc.ca/locations-contacts\' target="_blank"><span style="color:#009cde !important; text-decoration: underline">the court</span></a> for one. </span>'},
            {
              data: ' <span class="text-dark">You\’ll need to have a copy of it before you start your application.</span>',
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
          clickedContent: [
            {
              data: '<span class="text-dark"><strong>Great,</strong> to use the Child Support Recalculation Service, both you and the other parent or guardian must be B.C residents. You’re considered a B.C. resident if you have an address in the province and spend the majority of your time in the province.'
            }
          ],
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
      label: 'Has the financial situation of you or the other parent or guardian changed since your support order or written agreement was made? (In other words, has the annual income amount in your most recent support order or written agreement gone up or down?)',
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
          label: 'I don\’t know',
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
                liData: '<strong>Self-employment or partnership income;</strong>',
                tooltipData: 'Part or all of the paying parent\’s income is earned from carrying a trade or business as the owner, independent contractor, or some form of partnership.'
              },
              {
                liData: '<strong>Payor stands in place of a parent; or</strong>',
                tooltipData: 'A step-parent or person who is not the child\’s biological or adoptive parent.'
              },
              {
                liData: '<strong>Pattern of income</strong>',
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
              data: '<span class="text-dark">The Child Support Recalculation Service recalculates orders and agreements based on the child support tables only. Unlike a judge, it <strong>cannot</strong> consider any other factors in determining the support payable.',
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
              data: '<span class="text-dark">The Child Support Recalculation Service recalculates orders and agreements based on the child support tables only. Unlike a judge, it <strong>cannot</strong> consider any other factors in determining the support payable.',
            },
            {
              data: '<span class="text-dark">Since you have answered no to this question, your order or written agreement may be eligible for recalculation.</span>',
            },
          ],
        },
        {
          label: 'I don\’t know',
          clickedContent: [
            {
              data: '<span class="text-dark">The Child Support Recalculation Service recalculates orders and agreements based on the child support tables only. Unlike a judge, it <strong>cannot</strong> consider any other factors in determining the support payable.',
            },
            {
              data: '<span class="text-dark">If your child support was based on any of the above, the recalculation service <strong>cannot</strong> recalculate it and you are not eligible for the service.',
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
      label: 'Was the amount of child support in your order based on imputed income?',
      toolTip: ' Imputed income is when a judge decides the amount of income that child support should be based on. For example, a judge may decide to impute income if a person is intentionally underemployed or unemployed or has failed to provide income information when legally required to do so.',
      content: [],
      buttons: [
        {
          label: 'Yes',
          clickedContent: [
            {
              data: '<span class="text-dark">The Child Support Recalculation Service <strong>cannot</strong> recalculate support when a paying parent’s income has been imputed. This is because the recalculation service cannot make decisions that a judge can make about why income was imputed when child support was determined. If income has been imputed, your court order will usually say so.',
            },
            {
              data: '<span class="text-dark"><strong>There is one exception:</strong> If the order states that the paying parent’s income was imputed because they are exempt from paying federal or provincial income tax, then the recalculation service may be able to recalculate.',
            },
          ],
        },
        {
          label: 'No',
          clickedContent: [
            {
              data: '<span class="text-dark">The Child Support Recalculation Service cannot recalculate support when a paying paying parent’s income has been imputed.',
            },
            {
              data: '<span class="text-dark">Since you have answered no / not applicable to this question, your order or written agreement may be eligible for recalculation.',

            }
          ],
        },
        {
          label: 'I don\’t know',
          clickedContent: [
            {
              data: '<span class="text-dark">The Child Support Recalculation Service cannot recalculate support when a paying paying parent’s income was imputed.',
            },
            {
              data: '<span class="text-dark">Since you have answered no / not applicable to this question, your order or written agreement may be eligible for recalculation.',

            }
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
      label: ' Does your order or written agreement require the payment of special or extraordinary expenses?',
      toolTip:'These are additional amounts to be paid over and above the base child support amount. Some examples of these expenses (often referred to as Section 7 expenses) include childcare, medical or dental premiums, healthcare costs.',
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
                      '<span class="text-dark">Both parent\’s incomes are stated in the order or written agreement; and',
                  },
                  {
                    liData:
                      '<span class="text-dark">Each parent\’s proportion owing in relation to their incomes is clearly set out',
                  },
                ],
              },
            },
            {
              data: '<span class="text-dark">The recalculation service must review your order or written agreement first to determine whether it can recalculate your special or extraordinary Section 7 expenses.',
            },
            {
              data: '<span class="text-dark">The recalculation service doesn\’t have the authority to determine whether an expense continues to be eligible and payable. It may only adjust each parent’s proportionate share owing based on updated incomes.',
            },
            {
              data: '<span class="text-dark">If parents <strong>cannot</strong> agree on the eligibility of expenses, they may ask a court to decide.',
            },
          ],
        },
        {
          label: 'No',
          clickedContent: [
            {
              data: '<span class="text-dark">Since you have answered no to this question, the recalculation of special or extraordinary expenses does not apply to your situation.',
            }
          ],
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
        'Does your order/written agreement include child support involving:',
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
                tooltipData: 'A person legally becomes an adult at 19 years of age in B.C. However, child support can continue beyond 19 if the child continues to be financially dependent under certain situations.',
              },
              {
                liData: '<strong>Shared parenting; or</strong>',
                tooltipData: 'Both parents provide primary care for the children at least 40% of the time over the course of a year.',

              },
              {
                liData: '<strong>Income over $150,000 ?</strong>',
                tooltipData: 'If a parent\’s annual income is over $150,000, a court may consider other factors, in addition to the Child Support Guideline Tables.  These factors include conditions, means and other circumstances of the children and both parent\’s financial ability to contribute to the support of the child(ren).'

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
              data: '<span class="text-dark">Since you have answered yes to this question, the recalculation service will need to review your order or written agreement to decide whether it is eligible for recalculation.',
            },
          ],
        },
        {
          label: 'No',
          clickedContent: [
            {
              data: '<span class="text-dark">These Child Support Recalculation Service recalculates orders and agreements based on the child support tables only. As these situations can sometimes involve other factors, the service will need to review each case for eligibility',
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
              @Inject(AppConfigService) private config) {

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
      } else if (node && (question.clicked === 'Yes' || question.clicked === 'I don\’t know')) {
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
      } else if ((question.clicked === 'Yes' || question.clicked === 'I don\’t know')) {
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
    if((question.clicked == buttonItem.label &&  question.clicked == 'I don\’t know')){
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
       myGreen = true;
     } else {
       myRed = true;
     }
     this.setUIconColor(i, question);

   }
    if ((question.clicked == buttonItem.label &&  question.clicked === 'I don\’t know')){
     myGreen = true
   }

    // tslint:disable-next-line: object-literal-key-quotes
    return { 'myGreen' : myGreen, 'myRed' : myRed  };
 }
  public async ngOnInit() {

    // it is temporary solution. Later Links will retreive from configuration file
    this.bceIdRegisterLink = 'https://www.test.bceid.ca/register/basic/account_details.aspx?type=regular&eServiceType=basic';
    this.oidcSecurityService.checkAuth().subscribe(({ isAuthenticated }) => {
      this.logger.log('info',`isAuthenticated = ${isAuthenticated}`);
      if (isAuthenticated === true)
      {
        this.router.navigate(['/welcomeuser']);
      }
    });
  }

  login() {
    this.oidcSecurityService.authorize();
  }

  logout() {
    this.oidcSecurityService.logoffAndRevokeTokens();
  }

  downloadApplication()
  {
    const link = document.createElement('a');
    link.download = "Application.pdf";
    link.href = "assets/Application.pdf";
    link.click();
  }

}
