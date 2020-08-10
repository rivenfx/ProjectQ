import { HttpHeaders, HttpRequest } from '@angular/common/http';
import { ITokenService } from '@delon/auth';
import { SettingsService } from '@delon/theme';
import { AppConsts } from '@shared';

/** 请求帮助类 */
export class RequestHelper {

  /** 多租户配置 */
  public static readonly multiTenancy = {
    key: 'Riven.Tenant',
  };

  /** 是否已经被初始化 */
  protected static inited = false;

  /** settings服务 */
  protected static settingsSer: SettingsService;


  /** 初始化 */
  static init(settingsSer: SettingsService) {
    if (RequestHelper.inited) {
      return;
    }
    RequestHelper.inited = true;
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
    modifiedHeaders = RequestHelper.addTenantHeader(modifiedHeaders);

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
  static addAuthorizationHeaders(headers: HttpHeaders, authorizationSchema?: string): HttpHeaders {
    let authorizationHeaders = headers ? headers.getAll('Authorization') : null;
    if (!authorizationHeaders) {
      authorizationHeaders = [];
    }
    if (!authorizationSchema) {
      authorizationSchema = 'Bearer';
    }

    if (!this.itemExists(authorizationHeaders, (item) => item.startsWith(authorizationSchema))) {

      const token = RequestHelper.settingsSer.getData(AppConsts.settings.token);
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

  /** 添加租户到请求头 */
  static addTenantHeader(headers: HttpHeaders): HttpHeaders {
    if (RequestHelper.settingsSer.user) {
      const tenant = RequestHelper.settingsSer.getData(RequestHelper.multiTenancy.key);
      if (tenant && headers && !headers.has('Tenant')) {
        headers = headers.set('Tenant', tenant);
      }
    }
    return headers;
  }

  /** 判断数组中是否存在某一项 */
  static itemExists<T>(items: T[], predicate: (item: T) => boolean): boolean {
    for (const item of items) {
      if (predicate(item)) {
        return true;
      }
    }
    return false;
  }

}
