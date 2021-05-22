import { Injector, OnInit, ViewChild, Directive, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ModalHelper } from '@delon/theme';
import {
  ColumnItemDto,
  DynamicPageServiceProxy,
  PageFilterItemDto,
  QueryCondition,
  SortCondition,
  SortType,
} from '@service-proxies/service-proxies';
import { ISampleTableAction } from '@shared/components/sample-table';
import { NzTableComponent } from 'ng-zorro-antd/table';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from './app-component-base';
import { ReuseTabService } from '@delon/abc/reuse-tab';
import { STChange } from '@delon/abc/st';

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
  /** 虚拟滚动 */
  virtual?: boolean;
  /** 虚拟项高度 */
  virtualItemSize?: number;
  /** 滚动 */
  scroll?: { y?: string; x?: string };
  // ==========================================
  /** 是否显示分页 */
  show?: boolean;
  /** 是否前端分页 */
  front?: boolean;
  /** 是否显示快速跳转 */
  showQuickJumper?: boolean;
  /** 显示页数据量切换 */
  showSize?: boolean;
  /** 页面数据量组,默认 [10, 20, 30, 40, 50] */
  pageSizes?: number[];
  /** 显示边框 */
  bordered?: boolean;
  /** 表格大小 */
  tableSize?: 'small' | 'middle' | 'default';
}

/** 查询分页数据 */
export interface IFetchPageData {
  skipCount: number;
  pageSize: number;
  queryConditions: QueryCondition[];
  sortConditions: SortCondition[];
  finishedCallback?: () => void;
  successCallback?: (result: IPagedResultDto) => void;
}

/** 分页响应结果 */
export interface IPagedResultDto {
  items: any[];
  total: number;
}

@Component({
  template: '',
})
// tslint:disable-next-line:component-class-suffix
export abstract class ListViewComponentBase<T> extends AppComponentBase implements OnInit {
  /** 自动计算表格高度 */
  protected autoTableHeight = true;
  /** 表格高度 */
  protected tableHeight = 240;
  /** 误差计算高度 */
  protected errorHeight = 0;

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
    virtual: true,
    virtualItemSize: 30.6,
    scroll: { x: '1300px', y: '240px' },
    //
    show: true,
    front: false,
    showQuickJumper: true,
    showSize: true,
    pageSizes: [10, 20, 30, 40, 50],
    //
    bordered: true,
    tableSize: 'small',
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
    // @ts-ignore
  @ViewChild('pageTable') pageTableRef: NzTableComponent;

  /** 模态框帮助类 */
  modalHelper: ModalHelper;

  /** 动态页面服务 */
  dynamicPageSer: DynamicPageServiceProxy;

  /** 激活路由 */
  activatedRoute: ActivatedRoute;

  /** 复用标签服务 */
  reuseTabSer: ReuseTabService;

  /** 筛选条件 */
  queryConditions: QueryCondition[] = [];

  /** 排序条件 */
  sortConditions: SortCondition[] = [];

  /** 选中的数据 */
  checkedData: T[] = [];

  constructor(injector: Injector) {
    super(injector);

    this.modalHelper = injector.get(ModalHelper);
    this.dynamicPageSer = injector.get(DynamicPageServiceProxy);
    this.activatedRoute = injector.get(ActivatedRoute);
    this.reuseTabSer = injector.get(ReuseTabService);

    // 获取筛选条件配置名称名称
    const activatedRoute = injector.get(ActivatedRoute);
    if (activatedRoute.snapshot.data && activatedRoute.snapshot.data.permissions) {
      const permissions = activatedRoute.snapshot.data.permissions;
      if (Array.isArray(permissions) && permissions.length > 0) {
        this.pageName = permissions[0];
      } else if (typeof permissions === 'string') {
        this.pageName = permissions;
      }
    }

    setTimeout(() => {
      // 刷新复用标签
      this.reuseTabSer.refresh();
    }, 500);
  }

  ngOnInit(): void {
    this.calculatedHeight();
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
    if (!Array.isArray(this.checkedData)) {
      this.checkedData = [];
    }
  }

  /** 筛选条件初始化完成 */
  onFilterReady(queryConditions: QueryCondition[]) {
    this.onFilterChange(queryConditions);
    this.refresh();
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
    this.fetchData({
      // tslint:disable-next-line: object-literal-shorthand
      skipCount: skipCount,
      pageSize: this.pageInfo.size,
      queryConditions: this.queryConditions ?? [],
      sortConditions: this.sortConditions ?? [],
      finishedCallback: () => {
        this.loading = false;
      },
      successCallback: (result) => {
        if (!result || !result.items) {
          this.pageInfo.viewRecord = [];
          this.pageInfo.totalRecord = 0;
        } else {
          this.pageInfo.viewRecord = result.items;
          this.pageInfo.totalRecord = result.total;
        }
      },
    });
  }

  /** st change事件 */
  stChange(e: STChange): void {
    switch (e.type) {
      case 'checkbox': // 多选
        const checked = e.checkbox;
        this.onCheckChange(checked);
        break;
      case 'radio': // 单选
        this.onCheckChange([e.radio]);
        break;
      case 'sort': // 排序
        this.onSortChange(
          this.toSortConditions(e),
        );
        break;
      case 'pi': // page index 修改
        this.onPageIndexChange(e.pi);
        break;
      case 'ps': // page size 修改
        this.onPageSizeChange(e.ps);
        break;
    }
  }

  /** STChange 转 sort cond */
  protected toSortConditions(e: STChange): SortCondition[] {
    if (e.type !== 'sort') {
      return [];
    }

    const sortConditions: SortCondition[] = [];
    let sortType: SortType = SortType.None;
    let index = 0;
    for (const key in e.sort.map) {
      switch (e.sort.map[key]){
        case 'descend':
          sortType= SortType.Desc;
          break;
        case 'ascend':
          sortType= SortType.Asc;
          break;
      }

      sortConditions.push(
        new SortCondition({
          field: key,
          order: index++,
          type: sortType,
        }),
      );
    }
    return sortConditions;
  }


  /** 当 pageName 发生修改 */
  protected onPageNameChange(name: string) {
    this.fetchDynamicPageInfo(name);
  }

  /** 获取动态页面信息 pageFilter和columns */
  protected fetchDynamicPageInfo(name: string, callback?: () => void) {
    this.dynamicPageSer
      .getDynamicPageInfo(name)
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe((res) => {
        this.pageInfo.pageFilters = res.pageFilters;
        this.pageInfo.columns = res.columns;
        if (callback) {
          callback();
        }
      });
  }

  /** 获取pageFilterList */
  protected fetchPageFilter(name: string, callback?: () => void) {
    this.loading = true;
    this.dynamicPageSer
      .getPageFilters(name)
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
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
  protected fetchColumn(name: string, callback?: () => void) {
    this.loading = true;
    this.dynamicPageSer
      .getColumns(name)
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
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

  /** 计算列表高度 */
  protected calculatedHeight() {
    // TODO: 动态计算高度需要兼容移动端
    this.tableHeight = window.innerHeight - 64 - 41 - 101.8 - 32 - 32 - 120 - this.errorHeight;
    if (this.autoTableHeight) {
      const oldScroll = this.pageInfo.scroll;
      this.pageInfo.scroll = {
        x: oldScroll.x,
        y: this.tableHeight + 'px',
      };
    }
  }

  /** 加载列表数据 */
  abstract fetchData(fetch: IFetchPageData);
}
