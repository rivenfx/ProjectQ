import { CommonModule } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { SessionService } from './session.service';

const COMPONENTS = [

];

const DIRECTIVES = [

];

@NgModule({
  imports: [
    CommonModule
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

  static forRoot(): ModuleWithProviders<RivenModule> {
    return {
      ngModule: RivenModule,
      providers: [
        SessionService,
      ],
    };
  }
}
