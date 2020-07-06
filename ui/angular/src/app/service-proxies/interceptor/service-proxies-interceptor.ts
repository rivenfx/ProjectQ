import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Observable, Subject, of } from 'rxjs';

import {
  HttpClient,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpEvent,
  HttpResponse,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { SettingsService } from '@delon/theme';
import { ServiceProxiesInterceptorConfiguration } from './service-proxies-interceptor-configuration';
import { MessageService } from '@shared';


/**
 * 代理类http请求拦截器
 */
@Injectable()
export class ServiceProxiesInterceptor implements HttpInterceptor {

  protected configuration: ServiceProxiesInterceptorConfiguration;

  constructor(
    @Inject(DA_SERVICE_TOKEN) public tokenService: ITokenService,
    private settings: SettingsService,
    public messageSer: MessageService,
  ) {
    this.configuration = new ServiceProxiesInterceptorConfiguration(messageSer);
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    let interceptObservable = new Subject<HttpEvent<any>>();
    let modifiedRequest = this.normalizeRequestHeaders(request);

    next.handle(modifiedRequest)
      .subscribe((event: HttpEvent<any>) => {
        this.handleSuccessResponse(event, interceptObservable);
      }, (error: any) => {
        return this.handleErrorResponse(error, interceptObservable);
      });

    return interceptObservable;
  }

  protected normalizeRequestHeaders(request: HttpRequest<any>): HttpRequest<any> {
    let modifiedHeaders = new HttpHeaders();
    modifiedHeaders = request.headers.set('Pragma', 'no-cache')
      .set('Cache-Control', 'no-cache')
      .set('Expires', 'Sat, 01 Jan 2000 00:00:00 GMT');

    modifiedHeaders = this.addXRequestedWithHeader(modifiedHeaders);
    modifiedHeaders = this.addAuthorizationHeaders(modifiedHeaders);
    modifiedHeaders = this.addAspNetCoreCultureHeader(modifiedHeaders);
    modifiedHeaders = this.addAcceptLanguageHeader(modifiedHeaders);
    modifiedHeaders = this.addTenantIdHeader(modifiedHeaders);

    return request.clone({
      headers: modifiedHeaders,
    });
  }

  /**
   * 请求头添加 ajax 标识
   * @param headers
   */
  protected addXRequestedWithHeader(headers: HttpHeaders): HttpHeaders {
    if (headers) {
      headers = headers.set('X-Requested-With', 'XMLHttpRequest');
    }

    return headers;
  }

  /**
   * 请求头添加 asp.net core 国际化标识
   * @param headers
   */
  protected addAspNetCoreCultureHeader(headers: HttpHeaders): HttpHeaders {
    const lang = this.settings.layout.lang;
    if (lang && headers && !headers.has('.AspNetCore.Culture')) {
      headers = headers.set('.AspNetCore.Culture', lang);
    }

    return headers;
  }

  /**
   * 请求头添加 Accept-Language 国际化标识
   * @param headers
   */
  protected addAcceptLanguageHeader(headers: HttpHeaders): HttpHeaders {
    const lang = this.settings.layout.lang;
    if (lang && headers && !headers.has('Accept-Language')) {
      headers = headers.set('Accept-Language', lang);
    }
    return headers;
  }

  protected addTenantIdHeader(headers: HttpHeaders): HttpHeaders {
    // let cookieTenantIdValue = this._utilsService.getCookieValue('Abp.TenantId');
    // if (cookieTenantIdValue && headers && !headers.has('Abp.TenantId')) {
    //   headers = headers.set('Abp.TenantId', cookieTenantIdValue);
    // }

    return headers;
  }

  /**
   * 请求头添加 Authorization
   * @param headers
   */
  protected addAuthorizationHeaders(headers: HttpHeaders): HttpHeaders {
    let authorizationHeaders = headers ? headers.getAll('Authorization') : null;
    if (!authorizationHeaders) {
      authorizationHeaders = [];
    }
    if (!this.itemExists(authorizationHeaders, (item: string) => item.indexOf('Bearer ') == 0)) {
      let token = this.tokenService.get().token;
      if (headers && token) {
        headers = headers.set('Authorization', 'Bearer ' + token);
      }
    }

    return headers;
  }

  /**
   * 处理成功的响应
   * @param event
   * @param interceptObservable
   */
  protected handleSuccessResponse(event: HttpEvent<any>, interceptObservable: Subject<HttpEvent<any>>): void {
    const self = this;
    if (event instanceof HttpResponse) {
      if (event.body instanceof Blob && event.body.type && event.body.type.indexOf('application/json') >= 0) {
        const clonedResponse = event.clone();

        self.configuration.blobToText(event.body).subscribe(json => {
          const responseBody = json == 'null' ? {} : JSON.parse(json);
          debugger
          const modifiedResponse = self.configuration.handleResponse(event.clone({
            body: responseBody,
          }));

          interceptObservable.next(modifiedResponse.clone({
            body: new Blob([JSON.stringify(modifiedResponse.body)], { type: 'application/json' }),
          }));

          interceptObservable.complete();
        });
      } else {
        interceptObservable.next(event);
        interceptObservable.complete();
      }
    } else {
      interceptObservable.next(event);
    }
  }

  /**
   * 处理异常的响应
   * @param error
   * @param interceptObservable
   */
  protected handleErrorResponse(error: any, interceptObservable: Subject<HttpEvent<any>>): Observable<any> {
    const errorObservable = new Subject<any>();

    if (!(error.error instanceof Blob)) {
      interceptObservable.error(error);
      interceptObservable.complete();
      return of({});
    }

    this.configuration.blobToText(error.error).subscribe(json => {
      const errorBody = (json == '' || json == 'null') ? {} : JSON.parse(json);
      const errorResponse = new HttpResponse({
        headers: error.headers,
        status: error.status,
        body: errorBody,
      });

      const ajaxResponse = this.configuration.getAjaxResponseOrNull(errorResponse);

      if (ajaxResponse != null) {
        this.configuration.handleResponse(errorResponse);
      } else {
        this.configuration.handleNonWrapErrorResponse(errorResponse);
      }

      errorObservable.complete();

      interceptObservable.error(error);
      interceptObservable.complete();
    });

    return errorObservable;
  }

  private itemExists<T>(items: T[], predicate: (item: T) => boolean): boolean {
    for (let i = 0; i < items.length; i++) {
      if (predicate(items[i])) {
        return true;
      }
    }

    return false;
  }
}
