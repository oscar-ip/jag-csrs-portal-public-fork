import { inject, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { ConfigService } from './config.service';

describe('ConfigService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        {
          provide: ConfigService,
        },
      ],
    })
  );

  it('should create', inject([ConfigService], (service: ConfigService) => {
    expect(service).toBeTruthy();
  }));

});
