import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { TableBarComponent } from './table-bar.component';

describe('TableBarComponent', () => {
  let component: TableBarComponent;
  let fixture: ComponentFixture<TableBarComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ TableBarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TableBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
