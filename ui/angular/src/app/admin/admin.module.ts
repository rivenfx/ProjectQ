import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '@shared';
import { PageFilterModule } from '@shared/components/page-filter';
import { SampleComponentsModule } from '@shared/components/sample-components';
import { TableBarModule } from '@shared/components/table-bar';
import { RivenModule } from '@shared/riven';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminSharedModule } from './admin-shared';
import { DashboardComponent } from './dashboard';
import { RoleComponent } from './role';
import { CreateOrEditRoleComponent } from './role/create-or-edit-role';
import { UserComponent } from './user';
import { CreateOrEditUserComponent } from './user/create-or-edit-user';
import { SampleTableModule } from '@shared/components/sample-table';
import { TenantComponent } from './tenant';


/** entry的组件 */
const ENTRY_COMPONENTS = [
  CreateOrEditRoleComponent,
  CreateOrEditUserComponent,
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
    AdminSharedModule,
    RivenModule.forChild(),
    PageFilterModule,
    TableBarModule,
    SampleComponentsModule,
    SampleTableModule,
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
