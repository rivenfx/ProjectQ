import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { DashboardComponent } from './dashboard';
import { RoleComponent } from './role';
import { UserComponent } from './user';
import { STModule } from '@delon/abc';
import { FormsModule } from '@angular/forms';
import { NzFormModule, NzTableModule } from 'ng-zorro-antd';
import { SharedModule } from '@shared';
import { AdminSharedModule } from './admin-shared';
import { CreateOrEditRoleComponent } from './role/create-or-edit-role';
import { CreateOrEditUserComponent } from './user/create-or-edit-user';
import { RivenModule } from '@shared/riven';

const entryComponents = [
  CreateOrEditRoleComponent,
  CreateOrEditUserComponent,
];

@NgModule({
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule,
    AdminSharedModule,
    RivenModule.forChild(),
  ],
  declarations: [
    DashboardComponent,
    RoleComponent,
    UserComponent,
    ...entryComponents,
  ],
  entryComponents: [
    ...entryComponents,
  ],
})
export class AdminModule {
}
