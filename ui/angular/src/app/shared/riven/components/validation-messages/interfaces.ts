export interface IErrorDef {
  /** 错误类型 */
  error: string;
  /** 本地化键值 */
  localizationKey: string;
  /** 显示用的标签 */
  label?: string;
  /** 错误属性 */
  errorProperty?: string;
}
