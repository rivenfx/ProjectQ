import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { SharedModule } from '@shared';
import { LoginComponent } from './login';
import { RivenModule } from '../shared/riven';


@NgModule({
  declarations: [
    LoginComponent,
  ],
  imports: [
    CommonModule,
    AccountRoutingModule,
    SharedModule,
    RivenModule.forChild()
  ],
})
export class AccountModule {
}
