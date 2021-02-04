import { TestBed } from '@angular/core/testing';

import { WebRequestsService } from './web-requests.service';

describe('WebRequestsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WebRequestsService = TestBed.get(WebRequestsService);
    expect(service).toBeTruthy();
  });
});
