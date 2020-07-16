import { MessageService } from '@shared';
import { IAjaxResponse, IErrorInfo } from '@service-proxies/interceptor/interfaces';
import { Observable, of, Subject } from 'rxjs';
import { HttpEvent, HttpResponse } from '@angular/common/http';

/** 响应帮助类 */
export class ResponseHelper {

  /** 初始化标识 */
  protected static inited = false;

  /** 消息服务 */
  protected static messageSer: MessageService;

  /** 预定义的错误 */
  static predefineErrors = {
    /** 默认 - 错误 */
    default: <IErrorInfo>{
      message: 'An error has occurred!',
      details: 'Error details were not sent by server.',
    },

    /** 默认 - 未授权错误 */
    error401: <IErrorInfo>{
      message: 'You are not authenticated!',
      details: 'You should be authenticated (sign in) in order to perform this operation.',
    },

    /** 默认 - 没有操作权限错误 */
    error403: <IErrorInfo>{
      message: 'You are not authorized!',
      details: 'You are not allowed to perform this operation.',
    },

    /** 默认 - 未找到错误 */
    error404: <IErrorInfo>{
      message: 'Resource not found!',
      details: 'The resource requested could not be found on the server.',
    },
  };

  /** 初始化 ResponseHelper */
  static init(messageSer: MessageService) {
    if (ResponseHelper.inited) {
      return;
    }
    ResponseHelper.inited = true;

    ResponseHelper.messageSer = messageSer;
  }

  /** 处理成功的响应 */
  static handleSuccessResponse(event: HttpEvent<any>, interceptObservable: Subject<HttpEvent<any>>): void {

    if (event instanceof HttpResponse) {
      if (event.body instanceof Blob && event.body.type && event.body.type.indexOf('application/json') >= 0) {
        var clonedResponse = event.clone();

        ResponseHelper.blobToText(event.body).subscribe(json => {
          const responseBody = json == 'null' ? {} : JSON.parse(json);

          var modifiedResponse = ResponseHelper.handleResponse(event.clone({
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

  /** 处理失败的响应 */
  static handleErrorResponse(error: any, interceptObservable: Subject<HttpEvent<any>>): Observable<any> {
    var errorObservable = new Subject<any>();

    if (!(error.error instanceof Blob)) {
      interceptObservable.error(error);
      interceptObservable.complete();
      return of({});
    }

    ResponseHelper.blobToText(error.error).subscribe(json => {
      const errorBody = (json == '' || json == 'null') ? {} : JSON.parse(json);
      const errorResponse = new HttpResponse({
        headers: error.headers,
        status: error.status,
        body: errorBody,
      });

      const ajaxResponse = ResponseHelper.getAjaxResponseOrNull(errorResponse);

      if (ajaxResponse != null) {
        ResponseHelper.handleResponse(errorResponse);
      } else {
        ResponseHelper.handleNonWrapErrorResponse(errorResponse);
      }

      errorObservable.complete();

      interceptObservable.error(error);
      interceptObservable.complete();
    });

    return errorObservable;
  }

  /** 处理未包装响应的错误 */
  static handleNonWrapErrorResponse(response: HttpResponse<any>) {
    switch (response.status) {
      case 401:
        ResponseHelper.handleUnAuthorizedRequest(
          ResponseHelper.showError(ResponseHelper.predefineErrors.error401),
          '/',
        );
        break;
      case 403:
        ResponseHelper.showError(ResponseHelper.predefineErrors.error403);
        break;
      case 404:
        ResponseHelper.showError(ResponseHelper.predefineErrors.error404);
        break;
      default:
        ResponseHelper.showError(ResponseHelper.predefineErrors.default);
        break;
    }
  }

  /** blob转换为文本 */
  static blobToText(blob: any): Observable<string> {
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

  /** 处理响应 */
  static handleResponse(response: HttpResponse<any>): HttpResponse<any> {
    let ajaxResponse = ResponseHelper.getAjaxResponseOrNull(response);
    if (typeof (ajaxResponse) === 'undefined') {
      return response;
    }

    return ResponseHelper.handleAjaxResponse(response, ajaxResponse);
  }

  /** 处理ajax响应 提示错误/跳转链接/转换响应 */
  static handleAjaxResponse(response: HttpResponse<any>, ajaxResponse: IAjaxResponse): HttpResponse<any> {
    let newResponse: HttpResponse<any>;

    if (ajaxResponse.success) {

      newResponse = response.clone({
        body: ajaxResponse.result,
      });

      if (ajaxResponse.targetUrl) {
        ResponseHelper.handleTargetUrl(ajaxResponse.targetUrl);
      }
    } else {
      newResponse = response.clone({
        body: ajaxResponse.result,
      });

      if (!ajaxResponse.result) {
        ajaxResponse.result = ResponseHelper.predefineErrors.default;
      }

      this.logError(ajaxResponse.result);
      this.showError(ajaxResponse.result);

      if (response.status === 401) {
        ResponseHelper.handleUnAuthorizedRequest(null, ajaxResponse.targetUrl);
      }
    }

    return newResponse;
  }

  /** 尝试将响应转换为 IAjaxResponse 实例 */
  static getAjaxResponseOrNull(response: HttpResponse<any>): IAjaxResponse | null {
    if (!response || !response.headers) {
      return undefined;
    }

    const contentType = response.headers.get('Content-Type');
    if (!contentType) {
      console.warn('Content-Type is not sent!');
      return undefined;
    }

    if (contentType.indexOf('application/json') < 0) {
      console.warn('Content-Type is not application/json: ' + contentType);
      return undefined;
    }

    const responseObj = JSON.parse(JSON.stringify(response.body));
    if (!responseObj.__wrap) {
      return undefined;
    }

    return responseObj as IAjaxResponse;
  }

  /** 处理未授权请求 */
  static handleUnAuthorizedRequest(messagePromise: any, targetUrl?: string) {
    const self = this;

    if (messagePromise) {
      messagePromise.done(() => {
        this.handleTargetUrl(targetUrl || '/');
      });
    } else {
      self.handleTargetUrl(targetUrl || '/');
    }
  }

  /** 处理目标地址 */
  static handleTargetUrl(targetUrl: string): void {
    if (!targetUrl) {
      location.href = '/';
    } else {
      location.href = targetUrl;
    }
  }

  /** 日志 - 错误 */
  static logError(error: IErrorInfo): void {
    console.error(error);
  }

  /** 提示 - 错误 */
  static showError(error: IErrorInfo) {
    if (error.details) {
      ResponseHelper.messageSer.error(error.details, error.message || ResponseHelper.predefineErrors.default.message);
    } else {
      ResponseHelper.messageSer.error(error.message || ResponseHelper.predefineErrors.default.message);
    }
  }

}
