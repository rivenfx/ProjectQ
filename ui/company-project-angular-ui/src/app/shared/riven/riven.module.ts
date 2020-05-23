import { NgModule } from '@angular/core';
import { SessionService } from './session.service';
import { PermissionCheckerService } from './permission-checker.service';
import { AuthService } from './auth.service';
import { AppInfoService } from './app-info.service';

;

@NgModule({
  providers: [
    SessionService,
    PermissionCheckerService,
    AuthService,
    AppInfoService,
  ],
})
export class RivenModule {

}
