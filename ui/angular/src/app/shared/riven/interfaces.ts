/**
 * app info
 */
export interface IAppInfo {

  /** 名称 */
  name: string;

  /** 版本 */
  version: string;

  /** 设置 */
  settings: { key: string, value: any };

  /** 本地化 */
  localization: ILocalization;

  /** 权限 */
  claims: IClaims;
}

/** 当前登录用户信息 */
export interface ICurrentUserInfo {
  /** 用户id */
  id: number | string;
  /** 用户名称 */
  userName: string;
}

/** 本地化 */
export interface ILocalization {
  /** 所有语言 */
  languages: ILanguage[];
  /** 当前语言 */
  currentLanguage: ILanguage;
}

/** 语言信息 */
export interface ILanguage {
  /** 语言名称 */
  culture: string;
  /** 显示名称 */
  displayName: string;
  /** 图标 */
  icon: string;
  /** 本地化键值 */
  texts: { key: string, value: string };
}

/** 授权信息 */
export interface IClaims {
  /** 所有权限名称 */
  allClaims: string[];
  /** 拥有的权限名称 */
  grantedClaims: string[];
}
