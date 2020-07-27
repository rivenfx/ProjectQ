import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TenantChangeModal.ComponentComponent } from './tenant-change-modal.component';

describe('TenantChangeModal.ComponentComponent', () => {
  let component: TenantChangeModal.ComponentComponent;
  let fixture: ComponentFixture<TenantChangeModal.ComponentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TenantChangeModal.ComponentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TenantChangeModal.ComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
