import { TestBed } from '@angular/core/testing';

import { SampleTableDataProcessorService } from './sample-table-data-processor.service';

describe('SampleTableDataProcessorService', () => {
  let service: SampleTableDataProcessorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SampleTableDataProcessorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
