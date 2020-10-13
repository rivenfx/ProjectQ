import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SampleFormItemComponent } from './sample-form-item.component';

describe('SampleFormItemComponent', () => {
  let component: SampleFormItemComponent;
  let fixture: ComponentFixture<SampleFormItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SampleFormItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SampleFormItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
