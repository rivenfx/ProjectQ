import { Inject, Injectable, Injector } from '@angular/core';
import { Observable, Subject } from 'rxjs';

import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { SettingsService } from '@delon/theme';
import { MessageService } from '@shared/riven';
import { RequestHelper, ResponseHelper } from '@shared/riven/helper';
import { AppConsts } from '@shared';


/**
 * 代理类http请求拦截器
 */
@Injectable()
export class ServiceProxiesInterceptor implements HttpInterceptor {

  constructor(
    injector: Injector
  ) {
    RequestHelper.init(injector);
    ResponseHelper.init(injector);
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const interceptObservable = new Subject<HttpEvent<any>>();
    const modifiedRequest = RequestHelper.normalizeRequestHeaders(request);

    next.handle(modifiedRequest)
      .subscribe((event: HttpEvent<any>) => {
        ResponseHelper.handleSuccessResponse(event, interceptObservable);
      }, (error: any) => {
        return ResponseHelper.handleErrorResponse(error, interceptObservable);
      });

    return interceptObservable;
  }

}
