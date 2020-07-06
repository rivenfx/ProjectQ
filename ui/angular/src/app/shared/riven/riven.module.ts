import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SessionService } from './session.service';
import { PermissionCheckerService } from './permission-checker.service';
import { ValidationMessagesComponent } from './components/validation-messages';
import { MessageService } from './message.service';
import { NotifyService } from './notify.service';

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
        MessageService,
        NotifyService,
      ],
    };
  }
}
