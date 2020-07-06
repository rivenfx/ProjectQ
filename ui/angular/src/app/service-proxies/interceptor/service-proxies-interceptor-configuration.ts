import { IAjaxResponse, IErrorInfo } from './interfaces';
import { HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MessageService } from '@shared/riven';

/**
 * 代理类http请求拦截器配置
 * */
export class ServiceProxiesInterceptorConfiguration {

  /** 默认 - 错误 */
  defaultError = <IErrorInfo>{
    message: 'An error has occurred!',
    details: 'Error details were not sent by server.',
  };

  /** 默认 - 未授权错误 */
  defaultError401 = <IErrorInfo>{
    message: 'You are not authenticated!',
    details: 'You should be authenticated (sign in) in order to perform this operation.',
  };

  /** 默认 - 没有操作权限错误 */
  defaultError403 = <IErrorInfo>{
    message: 'You are not authorized!',
    details: 'You are not allowed to perform this operation.',
  };

  /** 默认 - 未找到错误 */
  defaultError404 = <IErrorInfo>{
    message: 'Resource not found!',
    details: 'The resource requested could not be found on the server.',
  };

  constructor(
    public messageSer: MessageService,
  ) {
  }

  /** 日志 - 错误 */
  logError(error: IErrorInfo): void {
    console.error(error);
  }

  /** 提示 - 错误 */
  showError(error: IErrorInfo) {
    if (error.details) {
      this.messageSer.error(error.details,error.message||this.defaultError.message);
    } else {
      this.messageSer.error(error.message || this.defaultError.message);
    }
  }

  /** 处理目标地址 */
  handleTargetUrl(targetUrl: string): void {
    if (!targetUrl) {
      location.href = '/';
    } else {
      location.href = targetUrl;
    }
  }

  /** 处理未授权请求 */
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

  /** 处理未包装响应的错误 */
  handleNonWrapErrorResponse(response: HttpResponse<any>) {
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

  /** 处理ajax响应 */
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

      if (!ajaxResponse.result) {
        ajaxResponse.result = this.defaultError;
      }

      this.logError(ajaxResponse.result);
      this.showError(ajaxResponse.result);

      if (response.status === 401) {
        this.handleUnAuthorizedRequest(null, ajaxResponse.targetUrl);
      }
    }

    return newResponse;
  }

  /** 获取ajax响应或非ajax响应 */
  getAjaxResponseOrNull(response: HttpResponse<any>): IAjaxResponse | null {
    debugger
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
    if (!responseObj.__wrap) {
      return null;
    }

    return responseObj as IAjaxResponse;
  }

  /** 处理响应 */
  handleResponse(response: HttpResponse<any>): HttpResponse<any> {
    let ajaxResponse = this.getAjaxResponseOrNull(response);
    if (ajaxResponse == null) {
      return response;
    }

    return this.handleAjaxResponse(response, ajaxResponse);
  }

  /** blob转换为文本 */
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
