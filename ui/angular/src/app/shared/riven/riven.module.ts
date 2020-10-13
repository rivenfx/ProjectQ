import { CommonModule } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { SharedModule } from '@shared';
import { SampleFormItemComponent } from './components/sample-form-item';
import { ValidationMessagesComponent } from './components/validation-messages';
import { ConfirmValidatorDirective } from './directives/confirm-validator';
import { MessageService } from './message.service';
import { NotifyService } from './notify.service';
import { PermissionCheckerService } from './permission-checker.service';
import { SessionService } from './session.service';

const COMPONENTS = [
  ValidationMessagesComponent,
  SampleFormItemComponent,
];

const DIRECTIVES = [
  ConfirmValidatorDirective,
];

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
  ],
  declarations: [
    ...COMPONENTS,
    ...DIRECTIVES,
  ],
  exports: [
    ...COMPONENTS,
    ...DIRECTIVES,
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
