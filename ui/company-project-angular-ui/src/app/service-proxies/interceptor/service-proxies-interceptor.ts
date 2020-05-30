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
import { IAjaxResponse, IErrorInfo } from './interfaces';
import { SettingsService } from '@delon/theme';


// @Injectable()
export class AbpHttpConfiguration {

  defaultError = <IErrorInfo>{
    message: 'An error has occurred!',
    details: 'Error details were not sent by server.',
  };

  defaultError401 = <IErrorInfo>{
    message: 'You are not authenticated!',
    details: 'You should be authenticated (sign in) in order to perform this operation.',
  };

  defaultError403 = <IErrorInfo>{
    message: 'You are not authorized!',
    details: 'You are not allowed to perform this operation.',
  };

  defaultError404 = <IErrorInfo>{
    message: 'Resource not found!',
    details: 'The resource requested could not be found on the server.',
  };

  logError(error: IErrorInfo): void {
    console.error(error);
  }

  showError(error: IErrorInfo): any {
    if (error.details) {
      return alert(error.details + (error.message || this.defaultError.message));
    } else {
      return alert(error.message || this.defaultError.message);
    }
  }

  handleTargetUrl(targetUrl: string): void {
    if (!targetUrl) {
      location.href = '/';
    } else {
      location.href = targetUrl;
    }
  }

  handleUnAuthorizedRequest(messagePromise: any, targetUrl?: string) {
    const self = this;

    if (messagePromise) {
      messagePromise.done(() => {
        this.handleTargetUrl(targetUrl || '/');
      });
    } else {
      self.handleTargetUrl(targetUrl || '/');
    }
  }

  handleNonAbpErrorResponse(response: HttpResponse<any>) {
    const self = this;

    switch (response.status) {
      case 401:
        self.handleUnAuthorizedRequest(
          self.showError(self.defaultError401),
          '/',
        );
        break;
      case 403:
        self.showError(self.defaultError403);
        break;
      case 404:
        self.showError(self.defaultError404);
        break;
      default:
        self.showError(self.defaultError);
        break;
    }
  }

  handleAjaxResponse(response: HttpResponse<any>, ajaxResponse: IAjaxResponse): HttpResponse<any> {
    let newResponse: HttpResponse<any>;

    if (ajaxResponse.success) {

      newResponse = response.clone({
        body: ajaxResponse.result,
      });

      if (ajaxResponse.targetUrl) {
        this.handleTargetUrl(ajaxResponse.targetUrl);
      }
    } else {

      newResponse = response.clone({
        body: ajaxResponse.result,
      });

      if (!ajaxResponse.error) {
        ajaxResponse.error = this.defaultError;
      }

      this.logError(ajaxResponse.error);
      this.showError(ajaxResponse.error);

      if (response.status === 401) {
        this.handleUnAuthorizedRequest(null, ajaxResponse.targetUrl);
      }
    }

    return newResponse;
  }

  getAjaxResponseOrNull(response: HttpResponse<any>): IAjaxResponse | null {
    if (!response || !response.headers) {
      return null;
    }

    const contentType = response.headers.get('Content-Type');
    if (!contentType) {
      console.warn('Content-Type is not sent!');
      return null;
    }

    if (contentType.indexOf('application/json') < 0) {
      console.warn('Content-Type is not application/json: ' + contentType);
      return null;
    }

    const responseObj = JSON.parse(JSON.stringify(response.body));
    // if (!responseObj.__abp) {
    //   return null;
    // }

    return responseObj as IAjaxResponse;
  }

  handleResponse(response: HttpResponse<any>): HttpResponse<any> {
    let ajaxResponse = this.getAjaxResponseOrNull(response);
    if (ajaxResponse == null) {
      return response;
    }

    return this.handleAjaxResponse(response, ajaxResponse);
  }

  blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
      if (!blob) {
        observer.next('');
        observer.complete();
      } else {
        let reader = new FileReader();
        reader.onload = function() {
          observer.next(this.result);
          observer.complete();
        };
        reader.readAsText(blob);
      }
    });
  }
}


@Injectable()
export class ServiceProxiesInterceptor implements HttpInterceptor {

  protected configuration = new AbpHttpConfiguration;

  constructor(
    @Inject(DA_SERVICE_TOKEN) public tokenService: ITokenService,
    private settings: SettingsService,
  ) {
    // this.configuration = configuration;
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
        this.configuration.handleNonAbpErrorResponse(errorResponse);
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
