import { HttpClientTestingModule } from '@angular/common/http/testing';
import { getTestBed, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { KeycloakService } from 'keycloak-angular';
//import { MockKeycloakService } from 'tests/mocks/mock-keycloak.service';

import { AuthGuard } from './auth-guard';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let injector: TestBed;
  let keycloakService: KeycloakService;
  const routeMock: any = { snapshot: {} };
  const routeStateMock: any = { snapshot: {}, url: '/cookies' };
  const routerMock = { navigate: jasmine.createSpy('navigate') };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthGuard,
        { provide: Router, useValue: routerMock },
      ],
    });
    injector = getTestBed();
    guard = injector.inject(AuthGuard);
    keycloakService = injector.inject(KeycloakService);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });


});
