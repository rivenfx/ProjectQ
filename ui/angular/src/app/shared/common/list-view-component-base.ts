import { Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ModalHelper } from '@delon/theme';
import {
  ColumnItemDto,
  ListViewServiceProxy,
  PageFilterItemDto,
  PageFilterServiceProxy,
  QueryCondition,
  SortCondition
} from '@service-proxies/service-proxies';
import { ISampleTableAction } from '@shared/components/sample-components/sample-table';
import { NzTableComponent } from 'ng-zorro-antd/table';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from './app-component-base';

/** 页面信息 */
export interface IPageInfo<T> {
  /** 名称 */
  name: string;
  // ==========================================
  /** 页码 */
  index: number;
  /** 一页最大数据量 */
  size: number;
  // ==========================================
  /** 筛选条件数据 */
  pageFilters?: PageFilterItemDto[];
  /** 列表列配置 */
  columns?: ColumnItemDto[];
  /** 列表数据 */
  viewRecord?: T[];
  /** 总数据量 */
  totalRecord?: number;
  // ==========================================
  /** 是否显示分页 */
  show?: boolean;
  /** 是否前端分页 */
  front?: boolean;
  /** 是否显示快速跳转 */
  showQuickJumper?: boolean;
  /** 页面数据量组,默认 [10, 20, 30, 40, 50] */
  pageSizes?: number[];


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
  get viewRecord(): T[] {
    return this.pageInfo.viewRecord;
  }

  /** 视图数据 */
  set viewRecord(input) {
    if (Array.isArray(input)) {
      this.pageInfo.viewRecord = input;
    } else {
      this.pageInfo.viewRecord = [];
    }
  }

  /** 数据总量 */
  get totalRecord(): number {
    return this.pageInfo.totalRecord;
  }

  /** 页面信息 */
  pageInfo: IPageInfo<T> = {
    name: undefined,
    //
    index: 1,
    size: 20,
    //
    pageFilters: [],
    columns: [],
    viewRecord: [],
    totalRecord: 0,
    //
    show: true,
    front: false,
    showQuickJumper: true,
    pageSizes: [10, 20, 30, 40, 50],
  };

  /** 表格滚动 */
  tableScroll: ITableScroll = {
    x: '100%',
    y: '100%',
  };

  /** 筛选条件/列表 配置名称 */
  set pageName(val: string) {
    this.pageInfo.name = val;
    if (val && val.trim() !== '') {
      this.onPageNameChange(val);
    }
  }
  /** 筛选条件/列表 配置名称 */
  get pageName(): string {
    return this.pageInfo.name;
  }

  /** 页面表格组件实例 */
  @ViewChild('pageTable') pageTableRef: NzTableComponent;

  /** 模态框帮助类 */
  modalHelper: ModalHelper;


  /** pageFilter查询器 */
  pageFilterSer: PageFilterServiceProxy;

  /** 列表配置查询器 */
  listViewSer: ListViewServiceProxy;

  /** 筛选条件 */
  queryConditions: QueryCondition[] = [];

  /** 排序条件 */
  sortConditions: SortCondition[] = [];

  /** 选中的数据 */
  checkedData: T[] = [];

  constructor(injector: Injector) {
    super(injector);

    this.modalHelper = injector.get(ModalHelper);
    this.pageFilterSer = injector.get(PageFilterServiceProxy);
    this.listViewSer = injector.get(ListViewServiceProxy);

    // 获取筛选条件配置名称名称
    const activatedRoute = injector.get(ActivatedRoute);
    if (activatedRoute.snapshot.data && activatedRoute.snapshot.data.claims) {
      const claims = activatedRoute.snapshot.data.claims;
      if (Array.isArray(claims) && claims.length > 0) {
        this.pageName = claims[0];
      } else if (typeof (claims) === 'string') {
        this.pageName = claims;
      }
    }
  }

  ngOnInit(): void {

  }

  /** 当触发操作事件 */
  onAction(event: ISampleTableAction) {
    if (!event) {
      return;
    }

    const eventFunc = (this as any)[event.name];
    if (eventFunc) {
      eventFunc.apply(this, [event.record]);
    } else {
      // tslint:disable-next-line: no-console
      console.debug(`action没有此函数 ${event.name}`);
    }
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

  /** 选中的数据发生更改 */
  onCheckChange(data: T[]) {
    this.checkedData = data;
  }

  /** 筛选条件初始化完成 */
  onFilterReady(queryConditions: QueryCondition[]) {
    this.onFilterChange(queryConditions);
    this.fetchListView(this.pageInfo.name, () => {
      this.refresh();
    });
  }

  /** 查询条件发生改变 */
  onFilterChange(queryConditions: QueryCondition[]) {
    this.queryConditions = queryConditions;
  }

  /** 排序条件发生改变 */
  onSortChange(sortConditions: SortCondition[]) {
    this.sortConditions = sortConditions;
    this.refresh();
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
        this.pageInfo.totalRecord = totalRecord;
      });
  }

  /** 获取pageFilterList */
  fetchPageFilter(name: string, callback?: () => void) {
    this.loading = true;
    this.pageFilterSer.getPageFilter(name)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        if (!res || !res.items) {
          this.pageInfo.pageFilters = [];
        } else {
          this.pageInfo.pageFilters = res.items;
        }
        if (callback) {
          callback();
        }
      });
  }

  /** 获取列表配置 */
  fetchListView(name: string, callback?: () => void) {
    this.loading = true;
    this.listViewSer.getListView(name)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        if (!res || !res.items) {
          this.pageInfo.columns = [];
        } else {
          this.pageInfo.columns = res.items;
        }
        if (callback) {
          callback();
        }
      });
  }

  /** pageName发生修改 */
  abstract onPageNameChange(name: string);

  /** 加载列表数据 */
  abstract fetchData(
    skipCount: number,
    pageSize: number,
    queryConditions: QueryCondition[],
    sortConditions: SortCondition[],
    callback: (totalRecord: number) => void
  );
}
