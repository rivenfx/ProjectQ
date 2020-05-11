import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { AccountLayoutComponent } from './account-layout';
import { JsonSchemaModule, SharedModule } from '@shared';
import { DelonFormModule } from '@delon/form';
import { LoginComponent } from './login';


@NgModule({
  declarations: [
    AccountLayoutComponent,
    LoginComponent,
  ],
  imports: [
    CommonModule,
    AccountRoutingModule,
    SharedModule
  ],
})
export class AccountModule {
}