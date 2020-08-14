import { TestBed } from '@angular/core/testing';

import { SampleDataSourceService } from './sample-data-source.service';

describe('SampleDataSourceService', () => {
  let service: SampleDataSourceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SampleDataSourceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
