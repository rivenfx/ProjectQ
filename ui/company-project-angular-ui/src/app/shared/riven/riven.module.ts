import { ModuleWithProviders, NgModule } from '@angular/core';
import { SessionService } from './session.service';
import { APIS, APIS_HTTP_INTERCEPTORS, ServiceProxyModule, SessionServiceProxy } from '../../service-proxies';

@NgModule({
  providers:[
    SessionService
  ]
})
export class RivenModule {

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: RivenModule,
      providers: [
        ...APIS,
        ...APIS_HTTP_INTERCEPTORS
      ],
    };
  }
}
