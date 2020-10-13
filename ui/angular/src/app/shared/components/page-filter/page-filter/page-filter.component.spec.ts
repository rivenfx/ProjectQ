import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageFilterComponent } from './page-filter.component';

describe('PageFilterComponent', () => {
  let component: PageFilterComponent;
  let fixture: ComponentFixture<PageFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
