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


@NgModule({
  declarations: [
    DashboardComponent,
    RoleComponent,
    UserComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule,
    AdminSharedModule,
  ],
})
export class AdminModule {
}
