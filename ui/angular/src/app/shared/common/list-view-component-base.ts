import { Injector, OnInit, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  ListViewInfoServiceProxy, PageColumnItemDto, PageFilterItemDto,
} from '@service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { IFetchPage2, ListComponentBase2 } from '@rivenfx/ng-page-filter';
import { STColumn } from '@delon/abc/st';
import { IPageFilterSchema } from '@rivenfx/ng-page-filter/page-filter';

@Component({
  template: '',
})
// tslint:disable-next-line:component-class-suffix
export abstract class ListViewComponentBase<T> extends ListComponentBase2<T> implements OnInit {


  /** 动态页面服务 */
  dynamicPageSer: ListViewInfoServiceProxy;

  /** 初始化filter配置 */
  templateFilterSchema: IPageFilterSchema = {
    basicRow: 1,
    hideLabel: false,
    configs: [],
  };
  /** 所有列配置 */
  templateColumns: STColumn[];


  constructor(injector: Injector) {
    super(injector);

    this.dynamicPageSer = injector.get(ListViewInfoServiceProxy);
    const activatedRoute = injector.get(ActivatedRoute);

    /** 初始化页面配置 */
    this.initViewConfigs();

    // 获取筛选条件配置名称名称
    if (activatedRoute.snapshot.data && activatedRoute.snapshot.data.permissions) {
      const permissions = activatedRoute.snapshot.data.permissions;
      if (Array.isArray(permissions) && permissions.length > 0) {
        this.viewName = permissions[0];
      } else if (typeof permissions === 'string') {
        this.viewName = permissions;
      }
    }
  }


  /** 当 viewName 发生修改 */
  viewNameChange(name: string): void {
    this.fetchViewConfigs(name);
  }


  /** 获取动态页面信息 pageFilter和columns */
  protected fetchViewConfigs(name: string, callback?: () => void) {
    this.dynamicPageSer
      .getListViewInfo(name)
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe((res) => {
        debugger
        // 处理筛选条件
        this.viewInfo.filterSchema = this.processViewFilters(res.filters);

        // 处理列表配置
        this.viewInfo.columns = this.processViewColumns(res.columns);

        // 回调函数
        if (callback) {
          callback();
        }
      });
  }

  /** 获取pageFilterList */
  protected fetchFilters(name: string, callback?: () => void) {
    this.loading = true;
    this.dynamicPageSer
      .getFilters(name)
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe((res) => {
        this.viewInfo.filterSchema = this.processViewFilters(res);
        if (callback) {
          callback();
        }
      });
  }

  /** 获取列表配置 */
  protected fetchColumns(name: string, callback?: () => void) {
    this.loading = true;
    this.dynamicPageSer
      .getColumns(name)
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe((res) => {
        // 处理列表配置
        this.viewInfo.columns = this.processViewColumns(res);

        // 回调函数
        if (callback) {
          callback();
        }
      });
  }

  /** 初始化页面配置 */
  abstract initViewConfigs();

  /** 处理filter配置 */
  protected processViewFilters(input: PageFilterItemDto[]): IPageFilterSchema {
    const tmpFilterSchema = {
      basicRow: this.templateFilterSchema.basicRow,
      hideLabel: this.templateFilterSchema.hideLabel,
      configs: [],
    };
    const filterConfigs = input.filter(o => !o.hidden)
      .sort((a, b) => a.order - b.order);

    if (filterConfigs.length === 0) { // 没有启用的配置
      tmpFilterSchema.configs = this.templateFilterSchema.configs;
    } else { // 遍历启用的配置
      for (const item of filterConfigs) {
        const newItem = this.templateFilterSchema.configs.find(o => o.field === item.field);
        if (!newItem) {
          continue;
        }
        newItem.order = item.order;
        tmpFilterSchema.configs.push(newItem);
      }
    }

    return tmpFilterSchema;
  }

  /** 处理列配置 */
  protected processViewColumns(input: PageColumnItemDto[]): STColumn[] {
    const tmpColumns = [];
    this.viewInfo.columns = [];
    const colunms = input.filter(o => !o.hidden)
      .sort((a, b) => a.order - b.order);
    if (colunms.length === 0) {
      tmpColumns.push(...this.templateColumns);
    } else {
      for (const item of colunms) {
        const newItem = this.templateColumns.find(o => o.index === item.field);
        if (!newItem) {
          continue;
        }
        tmpColumns.push(newItem);
      }
    }
    return this.processColumns(tmpColumns) ?? tmpColumns;
  }

  /** 处理页面列配置并返回处理后的结果 */
  abstract processColumns(columns: STColumn[]): STColumn[];
}
