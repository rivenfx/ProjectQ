import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '@shared';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminSharedModule } from './admin-shared';
import { DashboardComponent } from './dashboard';
import { RoleComponent } from './role';
import { CreateOrEditRoleComponent } from './role/create-or-edit-role';
import { UserComponent } from './user';
import { CreateOrEditUserComponent } from './user/create-or-edit-user';
import { TenantComponent } from './tenant';
import { EditTenantComponent } from './tenant/edit-tenant';
import { CreateTenantComponent } from './tenant/create-tenant';


/** entry的组件 */
const ENTRY_COMPONENTS = [
  CreateOrEditRoleComponent,
  CreateOrEditUserComponent,
  CreateTenantComponent,
  EditTenantComponent,
];

/** 所有组件 */
const ALL_COMPONENTS = [
  DashboardComponent,
  TenantComponent,
  RoleComponent,
  UserComponent,
  ...ENTRY_COMPONENTS,
];

@NgModule({
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule,
    AdminSharedModule
  ],
  declarations: [
    ...ALL_COMPONENTS,
  ],
  entryComponents: [
    ...ENTRY_COMPONENTS,
  ],
})
export class AdminModule {
}
