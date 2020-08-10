import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { I18nModule } from '@core/i18n';
import { SharedModule } from '@shared';
import { RivenModule } from '@shared/riven';
import { AccountRoutingModule } from './account-routing.module';
import { LoginComponent } from './login';
import { TenantChangeComponent } from './tenant-change';
import { TenantChangeModalComponent } from './tenant-change/tenant-change-modal';


@NgModule({
  declarations: [
    LoginComponent,
    TenantChangeComponent,
    TenantChangeModalComponent,
  ],
  imports: [
    CommonModule,
    I18nModule,
    AccountRoutingModule,
    SharedModule,
    RivenModule.forChild(),
  ],
})
export class AccountModule {
}
