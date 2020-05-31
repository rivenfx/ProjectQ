import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { SharedModule } from '@shared';
import { LoginComponent } from './login';
import { RivenModule } from '../shared/riven';
import { I18nModule } from '@core';


@NgModule({
  declarations: [
    LoginComponent,
  ],
  imports: [
    CommonModule,
    I18nModule,
    AccountRoutingModule,
    SharedModule,
    RivenModule.forChild()
  ],
})
export class AccountModule {
}
