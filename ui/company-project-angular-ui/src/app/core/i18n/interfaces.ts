export interface ILangData {
  /** 显示名称 */
  text: string;
  /** 图标 */
  icon?: string;
  /** angular使用的键值 */
  abbr: string;
  /** angular使用的数据 */
  ng: any;
  /** ng-zorro使用的数据 */
  zorro: any;
  /** 时间格式化使用的数据 */
  date: any;
  /** delon使用的数据 */
  delon: any;
}

export interface ILang {
  /** 语言编码 */
  code: string;
  /** 显示名称 */
  text: string;
  /** 图标 */
  icon?: string;
  /** angular使用的键值 */
  abbr: string;
}
