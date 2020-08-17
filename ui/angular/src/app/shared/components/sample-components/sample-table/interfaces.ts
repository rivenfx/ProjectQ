import { ColumnItemDto } from '@service-proxies';

/** 列表信息 */
export interface ISampleTableInfo {
  /** 列表数据 */
  data?: any[];
  /** 列表列 */
  columns?: ColumnItemDto[];
  /** 显示分页, 默认显示 */
  displayPagination?: boolean;
}

/** 当action触发 */
export interface ISampleTableAction {
  /** action名称 */
  name: string;
  /** 被点击的数据 */
  record: any;
}
