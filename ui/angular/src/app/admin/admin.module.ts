import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '@shared';
import { PageFilterModule } from '@shared/components/page-filter';
import { TableBarModule } from '@shared/components/table-bar';
import { RivenModule } from '@shared/riven';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminSharedModule } from './admin-shared';
import { DashboardComponent } from './dashboard';
import { RoleComponent } from './role';
import { CreateOrEditRoleComponent } from './role/create-or-edit-role';
import { UserComponent } from './user';
import { CreateOrEditUserComponent } from './user/create-or-edit-user';

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
    PageFilterModule,
    TableBarModule,
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
