export interface IPageFilterItem {
  /** 显示标签 */
  label: string | undefined;
  /** 字段 */
  field: string | undefined;
  /** 值 */
  value: string | undefined;
  /** 操作类型 */
  operator: 'Equal' | 'NotEqual' | 'Greater' | 'GreaterEqual' | 'Less' | 'LessEqual' | 'StartsWith' | 'EndsWith' | 'In' | 'NotIn' | 'Contains' | 'Between' | 'BetweenEqualStart' | 'BetweenEqualEnd' | 'BetweenEqualStartAndEnd';
  /** 空值是否跳过 */
  skipValueIsNull: boolean;
  /** 排序号 */
  order: number;
  /** 组件名称 */
  componentName: string | undefined;
  /** 组件参数 */
  args: { [key: string]: any; } | undefined;
  /** 值发生更改，触发的其它组件 */
  valueChange: string[] | undefined;
  /** 启用 */
  enabled: boolean;
  /** 宽度 */
  width: number;
  /** 宽度 */
  xsWidth: number | undefined;
  /** 宽度 */
  smWidth: number | undefined;
  /** 宽度 */
  mdWidth: number | undefined;
  /** 宽度 */
  lgWidth: number | undefined;
  /** 宽度 */
  xlWidth: number | undefined;
  /** 宽度 */
  xxlWidth: number | undefined;
}
export class PageFilterItem implements IPageFilterItem {
  order: number;
  label: string | undefined;
  componentName: string | undefined;
  args: { [key: string]: any; } | undefined;
  valueChange: string[] | undefined;
  enabled: boolean;
  width: number;
  xsWidth: number | undefined;
  smWidth: number | undefined;
  mdWidth: number | undefined;
  lgWidth: number | undefined;
  xlWidth: number | undefined;
  xxlWidth: number | undefined;
  field: string | undefined;
  value: string | undefined;
  operator: 'Equal' | 'NotEqual' | 'Greater' | 'GreaterEqual' | 'Less' | 'LessEqual' | 'StartsWith' | 'EndsWith' | 'In' | 'NotIn' | 'Contains' | 'Between' | 'BetweenEqualStart' | 'BetweenEqualEnd' | 'BetweenEqualStartAndEnd';
  skipValueIsNull: boolean;

  constructor(data?: IPageFilterItem) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          this[property] = data[property];
        }
      }
    }
  }

  init(_data?: any) {
    if (_data) {
      this.order = _data['order'];
      this.label = _data['label'];
      this.componentName = _data['componentName'];
      if (_data['args']) {
        this.args = {} as any;
        for (const key in _data['args']) {
          if (_data['args'].hasOwnProperty(key)) {
            this.args[key] = _data['args'][key];
          }
        }
      }
      if (Array.isArray(_data['valueChange'])) {
        this.valueChange = [] as any;
        for (const item of _data['valueChange']) {
          this.valueChange.push(item);
        }
      }
      this.enabled = _data['enabled'];
      this.width = _data['width'];
      this.xsWidth = _data['xsWidth'];
      this.smWidth = _data['smWidth'];
      this.mdWidth = _data['mdWidth'];
      this.lgWidth = _data['lgWidth'];
      this.xlWidth = _data['xlWidth'];
      this.xxlWidth = _data['xxlWidth'];
      this.field = _data['field'];
      this.value = _data['value'];
      this.operator = _data['operator'];
      this.skipValueIsNull = _data['skipValueIsNull'];
    }
  }



  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data['order'] = this.order;
    data['label'] = this.label;
    data['componentName'] = this.componentName;
    if (this.args) {
      data['args'] = {};
      for (const key in this.args) {
        if (this.args.hasOwnProperty(key)) {
          data['args'][key] = this.args[key];
        }
      }
    }
    if (Array.isArray(this.valueChange)) {
      data['valueChange'] = [];
      for (const item of this.valueChange) {
        data['valueChange'].push(item);
      }
    }
    data['enabled'] = this.enabled;
    data['width'] = this.width;
    data['xsWidth'] = this.xsWidth;
    data['smWidth'] = this.smWidth;
    data['mdWidth'] = this.mdWidth;
    data['lgWidth'] = this.lgWidth;
    data['xlWidth'] = this.xlWidth;
    data['xxlWidth'] = this.xxlWidth;
    data['field'] = this.field;
    data['value'] = this.value;
    data['operator'] = this.operator;
    data['skipValueIsNull'] = this.skipValueIsNull;
    return data;
  }

  clone(): PageFilterItem {
    const json = this.toJSON();
    const result = new PageFilterItem();
    result.init(json);
    return result;
  }

  // tslint:disable-next-line:member-ordering
  static fromJS(data: any): PageFilterItem {
    data = typeof data === 'object' ? data : {};
    const result = new PageFilterItem();
    result.init(data);
    return result;
  }
}

