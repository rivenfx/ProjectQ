import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import * as ServiceProxies from './service-proxies';
import { ServiceProxiesInterceptor } from './interceptor';
import { API_BASE_URL } from './service-proxies';
import { AppConsts } from '@shared/app-consts';
import { throwIfAlreadyLoaded } from '@core/module-import-guard';


export const APIS = [
  ServiceProxies.TokenAuthServiceProxy,
  ServiceProxies.UserServiceProxy,
  ServiceProxies.RoleServiceProxy,
  ServiceProxies.TenantServiceProxy,
  ServiceProxies.SessionServiceProxy,
  ServiceProxies.ClaimsServiceProxy,
  ServiceProxies.PageFilterServiceProxy
];

export const APIS_HTTP_INTERCEPTORS = [
  { provide: HTTP_INTERCEPTORS, useClass: ServiceProxiesInterceptor, multi: true },
];

@NgModule({
  imports: [
  ],
  providers: [
    ...APIS,
    ...APIS_HTTP_INTERCEPTORS,
  ],
})
export class ServiceProxyModule {

  constructor(@Optional() @SkipSelf() parentModule: ServiceProxyModule) {
    throwIfAlreadyLoaded(parentModule, 'ServiceProxyModule');
  }

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: ServiceProxyModule,
      providers: [
        { provide: API_BASE_URL, useFactory: () => AppConsts.remoteServiceUrl },
      ],
    };
  }

}
