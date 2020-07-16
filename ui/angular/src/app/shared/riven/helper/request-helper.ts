import { ITokenService } from '@delon/auth';
import { SettingsService } from '@delon/theme';
import { HttpHeaders, HttpRequest } from '@angular/common/http';

/** 请求帮助类 */
export class RequestHelper {

  /** 是否已经被初始化 */
  protected static inited = false;

  /** token服务 */
  protected static tokenSer: ITokenService;

  /** settings服务 */
  protected static settingsSer: SettingsService;


  /** 初始化 */
  static init(tokenSer: ITokenService, settingsSer: SettingsService) {
    if (RequestHelper.inited) {
      return;
    }
    RequestHelper.inited = true;

    RequestHelper.tokenSer = tokenSer;
    RequestHelper.settingsSer = settingsSer;
  }

  /** 标准化请求头 */
  static normalizeRequestHeaders(request: HttpRequest<any>, authorizationSchema: string = 'Bearer'): HttpRequest<any> {
    let modifiedHeaders = new HttpHeaders();
    modifiedHeaders = request.headers.set('Pragma', 'no-cache')
      .set('Cache-Control', 'no-cache')
      .set('Expires', 'Sat, 01 Jan 2000 00:00:00 GMT');

    modifiedHeaders = RequestHelper.addXRequestedWithHeader(modifiedHeaders);
    modifiedHeaders = RequestHelper.addAuthorizationHeaders(modifiedHeaders, authorizationSchema);
    modifiedHeaders = RequestHelper.addAspNetCoreCultureHeader(modifiedHeaders);
    modifiedHeaders = RequestHelper.addAcceptLanguageHeader(modifiedHeaders);
    modifiedHeaders = RequestHelper.addTenantIdHeader(modifiedHeaders);

    return request.clone({
      headers: modifiedHeaders,
    });
  }

  /** 请求头添加 ajax 标识 */
  static addXRequestedWithHeader(headers: HttpHeaders): HttpHeaders {
    if (headers) {
      headers = headers.set('X-Requested-With', 'XMLHttpRequest');
    }
    return headers;
  }

  /** 请求头添加认证标识 */
  static addAuthorizationHeaders(headers: HttpHeaders, authorizationSchema: string = undefined): HttpHeaders {
    let authorizationHeaders = headers ? headers.getAll('Authorization') : null;
    if (!authorizationHeaders) {
      authorizationHeaders = [];
    }
    if (!authorizationSchema) {
      authorizationSchema = 'Bearer';
    }

    if (!this.itemExists(authorizationHeaders, (item) => item.startsWith(authorizationSchema))) {
      let token = RequestHelper.tokenSer.get().token;
      if (headers && token) {
        headers = headers.set('Authorization', `${authorizationSchema} ${token}`);
      }
    }

    return headers;
  }

  /** 请求头添加 asp.net core .AspNetCore.Culture 国际化标识 */
  static addAspNetCoreCultureHeader(headers: HttpHeaders): HttpHeaders {
    const lang = RequestHelper.settingsSer.layout.lang;
    if (lang && headers && !headers.has('.AspNetCore.Culture')) {
      headers = headers.set('.AspNetCore.Culture', lang);
    }

    return headers;
  }

  /** 请求头添加 Accept-Language 国际化标识 */
  static addAcceptLanguageHeader(headers: HttpHeaders): HttpHeaders {
    const lang = RequestHelper.settingsSer.layout.lang;
    if (lang && headers && !headers.has('Accept-Language')) {
      headers = headers.set('Accept-Language', lang);
    }
    return headers;
  }

  /** 添加租户Id到请求头 */
  static addTenantIdHeader(headers: HttpHeaders): HttpHeaders {
    if (RequestHelper.settingsSer.user) {
      const tenantId = RequestHelper.settingsSer.user['tenantId'];
      if (tenantId && headers && !headers.has('TenantId')) {
        headers = headers.set('TenantId', tenantId);
      }
    }
    return headers;
  }

  /** 判断数组中是否存在某一项 */
  static itemExists<T>(items: T[], predicate: (item: T) => boolean): boolean {
    for (let i = 0; i < items.length; i++) {
      if (predicate(items[i])) {
        return true;
      }
    }

    return false;
  }

}
