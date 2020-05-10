import { ModuleWithProviders, NgModule } from '@angular/core';
import { SessionService } from './session.service';

@NgModule({})
export class RivenModule {

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: RivenModule,
      providers: [
        SessionService,
      ],
    };
  }
}
