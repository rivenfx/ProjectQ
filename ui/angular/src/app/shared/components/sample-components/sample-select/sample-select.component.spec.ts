import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { SampleSelectComponent } from './sample-select.component';

describe('SampleSelectComponent', () => {
  let component: SampleSelectComponent;
  let fixture: ComponentFixture<SampleSelectComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SampleSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SampleSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
