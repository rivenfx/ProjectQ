import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import * as ApiServiceProxies from './service-proxies';
import { SimpleInterceptor } from '@delon/auth';

const apiServiceProxies = [
  ApiServiceProxies.TokenAuthServiceProxy,
  ApiServiceProxies.UserServiceProxy,
  ApiServiceProxies.RoleServiceProxy,
];

@NgModule({
  providers: [
    ...apiServiceProxies,
    { provide: HTTP_INTERCEPTORS, useClass: SimpleInterceptor, multi: true },
  ],
})
export class ServiceProxyModule {
}
