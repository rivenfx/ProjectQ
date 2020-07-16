import { Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from './app-component-base';
import { NzTableComponent } from 'ng-zorro-antd';

/** 页面信息 */
export interface IPageInfo {
  /** 页码 */
  index: number;
  /** 一页最大数据量 */
  size: number;
}

/** 页面滚动信息 */
export interface ITableScroll {
  /** 宽度 */
  x: string;
  /** 高度 */
  y: string;
}

export abstract class ListViewComponentBase<T> extends AppComponentBase
  implements OnInit {

  /** 视图数据 */
  private _viewData: T[];

  /** 数据总量 */
  private _total: number = 0;

  /** 视图数据 */
  get viewData(): T[] {
    return this._viewData;
  }

  /** 视图数据 */
  set viewData(input) {
    if (Array.isArray(input)) {
      this._viewData = input;
    } else {
      this._viewData = [];
    }
  }

  /** 数据总量 */
  get total(): number {
    return this._total;
  }

  /** 页面信息 */
  pageInfo: IPageInfo = {
    index: 0,
    size: 20,
  };

  /** 表格滚动 */
  tableScroll: ITableScroll = {
    x: '100%',
    y: '100%',
  };

  /** 页面表格组件实例 */
  @ViewChild('pageTable') pageTableRef: NzTableComponent;

  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh();
  }

  /** 页码发生更改 */
  onPageIndexChange(pageIndex: number) {
    this.pageInfo.index = pageIndex;
    this.refresh();
  }

  /** 页面数据量发生改变 */
  onPageSizeChange(pageSize: number) {
    this.pageInfo.size = pageSize;
    this.refresh(true);
  }

  /** 刷新页面 */
  refresh(gotoFirstPage: boolean = false) {
    this.loading = true;
    if (gotoFirstPage) {
      this.pageInfo.index = 0;
    }

    const skipCount = this.pageInfo.index * this.pageInfo.size;
    this.fetchData(skipCount, this.pageInfo.size, (total) => {
      this._total = total;
    });
  }

  /** 加载数据 */
  abstract fetchData(skipCount: number, pageSize: number, callback: (total: number) => void);
}
