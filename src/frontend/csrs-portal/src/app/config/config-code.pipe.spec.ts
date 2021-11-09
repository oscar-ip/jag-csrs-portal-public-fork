import { TestBed, waitForAsync, inject } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { ConfigCodePipe } from './config-code.pipe';
import { ConfigService } from './config.service';
import { RouterTestingModule } from '@angular/router/testing';
import { Component } from '@angular/core';

// Added the declaration of BlankComponent to be used for routing
@Component({ selector: 'app-test-blank', template: `` })
class BlankComponent {}

describe('ConfigCodePipe', () => {
  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        imports: [
          RouterTestingModule.withRoutes([
            { path: 'ticket/find', component: BlankComponent },
          ]),
          HttpClientTestingModule,
        ],
        providers: [
          {
            provide: ConfigService,
          },
        ],
        declarations: [BlankComponent],
      });
    })
  );

});
