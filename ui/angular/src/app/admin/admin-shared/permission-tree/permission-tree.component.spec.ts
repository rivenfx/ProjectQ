import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { PermissionTreeComponent } from './permission-tree.component';

describe('PermissionTreeComponent', () => {
  let component: PermissionTreeComponent;
  let fixture: ComponentFixture<PermissionTreeComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ PermissionTreeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PermissionTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
