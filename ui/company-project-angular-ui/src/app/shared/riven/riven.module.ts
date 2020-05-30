import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { SessionService } from './session.service';
import { PermissionCheckerService } from './permission-checker.service';
import { AuthService } from './auth.service';
import { AppInfoService } from './app-info.service';
import { ValidationMessagesComponent } from './components/validation-messages';
import { CommonModule } from '@angular/common';
import { throwIfAlreadyLoaded } from '@core';

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
})
export class RivenModule {

  static forChild(): ModuleWithProviders {
    return {
      ngModule: RivenModule,
    };
  }

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: RivenModule,
      providers: [
        SessionService,
        PermissionCheckerService,
        AuthService,
        AppInfoService,
      ],
    };
  }
}
