import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';

import { throwIfAlreadyLoaded } from '@core/module-import-guard';
import { ServiceProxiesInterceptor } from '@rivenfx/ng-common';
import { AppConsts } from '@shared/app-consts';
import * as ServiceProxies from './service-proxies';
import { API_BASE_URL } from './service-proxies';


export const APIS = [
  ServiceProxies.TokenAuthServiceProxy,
  ServiceProxies.UserServiceProxy,
  ServiceProxies.RoleServiceProxy,
  ServiceProxies.TenantServiceProxy,
  ServiceProxies.SessionServiceProxy,
  ServiceProxies.PermissionServiceProxy,
  ServiceProxies.ListViewInfoServiceProxy
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

  static forRoot(): ModuleWithProviders<ServiceProxyModule> {
    return {
      ngModule: ServiceProxyModule,
      providers: [
        {
          provide: API_BASE_URL, useFactory: () => {
            return AppConsts.remoteServiceUrl;
          }
        },
      ],
    };
  }

}
