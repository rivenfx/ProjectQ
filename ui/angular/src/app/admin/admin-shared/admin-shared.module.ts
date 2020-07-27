import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared';
import { PermissionTreeComponent } from './permission-tree';


@NgModule({
  declarations: [
    PermissionTreeComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
  ],
  exports: [
    PermissionTreeComponent,
  ],
})
export class AdminSharedModule {
}
