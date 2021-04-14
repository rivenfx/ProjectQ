import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { RegisterTenantComponent } from './register-tenant.component';

describe('RegisterTenantComponent', () => {
  let component: RegisterTenantComponent;
  let fixture: ComponentFixture<RegisterTenantComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RegisterTenantComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterTenantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
