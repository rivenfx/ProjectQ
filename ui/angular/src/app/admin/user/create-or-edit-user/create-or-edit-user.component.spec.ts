import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CreateOrEditUserComponent } from './create-or-edit-user.component';

describe('CreateOrEditUserComponent', () => {
  let component: CreateOrEditUserComponent;
  let fixture: ComponentFixture<CreateOrEditUserComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateOrEditUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateOrEditUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
