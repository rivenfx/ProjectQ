import { ModuleWithProviders, NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { SimpleInterceptor } from '@delon/auth';

import * as ServiceProxies from './service-proxies';


export const APIS = [
  ServiceProxies.TokenAuthServiceProxy,
  ServiceProxies.UserServiceProxy,
  ServiceProxies.RoleServiceProxy,
];

export const APIS_HTTP_INTERCEPTORS = [
  { provide: HTTP_INTERCEPTORS, useClass: SimpleInterceptor, multi: true },
];

@NgModule({
  providers: [
    ...APIS,
    ...APIS_HTTP_INTERCEPTORS
  ],
})
export class ServiceProxyModule {

}
