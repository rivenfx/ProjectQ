import { Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ModalHelper } from '@delon/theme';
import { QueryCondition, SortCondition } from '@service-proxies';
import { NzTableComponent } from 'ng-zorro-antd/table';
import { AppComponentBase } from './app-component-base';

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
  private _viewRecord: T[];

  /** 数据总量 */
  private _totalRecord = 0;

  /** 视图数据 */
  get viewRecord(): T[] {
    return this._viewRecord;
  }

  /** 视图数据 */
  set viewRecord(input) {
    if (Array.isArray(input)) {
      this._viewRecord = input;
    } else {
      this._viewRecord = [];
    }
  }

  /** 数据总量 */
  get totalRecord(): number {
    return this._totalRecord;
  }

  /** 总页数 */
  get totalPage(): number {
    if (this.totalRecord <= 0) {
      return 0;
    } else {
      return (this.totalRecord + this.pageInfo.size - 1) / this.pageInfo.size;
    }
  }

  /** 页面信息 */
  pageInfo: IPageInfo = {
    index: 1,
    size: 20,
  };

  /** 表格滚动 */
  tableScroll: ITableScroll = {
    x: '100%',
    y: '100%',
  };

  /** 筛选条件配置名称 */
  pageFilterName: string;

  /** 页面表格组件实例 */
  @ViewChild('pageTable') pageTableRef: NzTableComponent;

  /** 模态框帮助类 */
  modalHelper: ModalHelper;

  /** 筛选条件 */
  queryConditions: QueryCondition[] = [];

  /** 排序条件 */
  sortConditions: SortCondition[] = [];

  constructor(injector: Injector) {
    super(injector);

    this.modalHelper = injector.get(ModalHelper);

    // 获取筛选条件配置名称名称
    const activatedRoute = injector.get(ActivatedRoute);
    if (activatedRoute.snapshot.data && activatedRoute.snapshot.data.claims) {
      const claims = activatedRoute.snapshot.data.claims;
      if (Array.isArray(claims) && claims.length > 0) {
        this.pageFilterName = claims[0];
      } else if (typeof (claims) === 'string') {
        this.pageFilterName = claims;
      }
    }
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
      this.pageInfo.index = 1;
    }

    const skipCount = (this.pageInfo.index - 1) * this.pageInfo.size;
    this.fetchData(
      skipCount,
      this.pageInfo.size,
      this.queryConditions,
      this.sortConditions,
      (totalRecord) => {
        this._totalRecord = totalRecord;
      });
  }

  /** 查询条件发生改变 */
  onFilterChange(queryConditions: QueryCondition[]) {
    this.queryConditions = queryConditions;
  }

  /** 排序条件发生改变 */
  onSortChange(sortConditions: SortCondition[]) {
    this.sortConditions = sortConditions;
  }

  /** 加载数据 */
  abstract fetchData(
    skipCount: number,
    pageSize: number,
    queryConditions: QueryCondition[],
    sortConditions: SortCondition[],
    callback: (totalRecord: number) => void
  );
}
