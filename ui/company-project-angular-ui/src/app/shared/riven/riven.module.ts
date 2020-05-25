import { NgModule } from '@angular/core';
import { SessionService } from './session.service';
import { PermissionCheckerService } from './permission-checker.service';
import { AuthService } from './auth.service';
import { AppInfoService } from './app-info.service';
import { ValidationMessagesComponent } from './components/validation-messages';
import { CommonModule } from '@angular/common';

const COMPONENTS = [
  ValidationMessagesComponent,
];

@NgModule({
  imports: [
    CommonModule,
  ],
  declarations: [
    ...COMPONENTS,
  ],
  exports: [
    ...COMPONENTS,
  ],
  providers: [
    SessionService,
    PermissionCheckerService,
    AuthService,
    AppInfoService,
  ],
})
export class RivenModule {

}
