import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { DashboardComponent } from './dashboard';
import { RoleComponent } from './role';
import { UserComponent } from './user';


@NgModule({
  declarations: [
    DashboardComponent,
    RoleComponent,
    UserComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
