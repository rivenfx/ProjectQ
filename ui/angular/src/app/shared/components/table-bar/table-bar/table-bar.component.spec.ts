import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TableBarComponent } from './table-bar.component';

describe('TableBarComponent', () => {
  let component: TableBarComponent;
  let fixture: ComponentFixture<TableBarComponent>;

  beforeEach(async(() => {
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
