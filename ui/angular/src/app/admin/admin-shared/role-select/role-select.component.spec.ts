import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { RoleSelectComponent } from './role-select.component';

describe('RoleSelectComponent', () => {
  let component: RoleSelectComponent;
  let fixture: ComponentFixture<RoleSelectComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RoleSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoleSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
