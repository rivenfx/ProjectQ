import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared';
import { PermissionTreeComponent } from './permission-tree';
import { RoleSelectComponent } from './role-select';


@NgModule({
  imports: [
    CommonModule,
    SharedModule,
  ],
  declarations: [
    PermissionTreeComponent,
    RoleSelectComponent,
  ],
  exports: [
    PermissionTreeComponent,
    RoleSelectComponent
  ],
})
export class AdminSharedModule {
}
