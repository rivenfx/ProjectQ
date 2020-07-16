import { Inject, Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

import {
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpEvent,
} from '@angular/common/http';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { SettingsService } from '@delon/theme';
import { MessageService } from '@shared';
import { RequestHelper, ResponseHelper } from '@shared/riven/helper';


/**
 * 代理类http请求拦截器
 */
@Injectable()
export class ServiceProxiesInterceptor implements HttpInterceptor {

  constructor(
    @Inject(DA_SERVICE_TOKEN) public tokenSer: ITokenService,
    private settingsSer: SettingsService,
    public messageSer: MessageService,
  ) {
    RequestHelper.init(tokenSer, settingsSer);
    ResponseHelper.init(messageSer);
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