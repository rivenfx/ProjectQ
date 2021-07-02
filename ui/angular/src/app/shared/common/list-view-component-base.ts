import { Injector, OnInit, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  ListViewInfoServiceProxy,
} from '@service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { IFetchPage2, ListComponentBase2 } from '@rivenfx/ng-page-filter';

@Component({
  template: '',
})
// tslint:disable-next-line:component-class-suffix
export abstract class ListViewComponentBase<T> extends ListComponentBase2<T> implements OnInit {



  /** 动态页面服务 */
  dynamicPageSer: ListViewInfoServiceProxy;



  constructor(injector: Injector) {
    super(injector);


    this.dynamicPageSer = injector.get(ListViewInfoServiceProxy);


    // 获取筛选条件配置名称名称
    const activatedRoute = injector.get(ActivatedRoute);
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
    this.fetchDynamicPageInfo(name);
  }


  /** 当 pageName 发生修改 */
  protected onPageNameChange(name: string) {
    this.fetchDynamicPageInfo(name);
  }

  /** 获取动态页面信息 pageFilter和columns */
  protected fetchDynamicPageInfo(name: string, callback?: () => void) {
    this.dynamicPageSer
      .getListViewInfo(name)
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe((res) => {
        // this.viewInfo.filterSchema = {
        //   configs: res.filters.map(o => {
        //     return {
        //       ...o,
        //       valueChange: [],
        //     }
        //   })
        // };
        // this.pageInfo.pageFilters = res.filters;
        // this.pageInfo.columns = res.columns;
        // if (callback) {
        //   callback();
        // }
      });
  }

  /** 获取pageFilterList */
  protected fetchPageFilter(name: string, callback?: () => void) {
    this.loading = true;
    this.dynamicPageSer
      .getFilters(name)
      .pipe(
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe((res) => {
        // if (!res || !res) {
        //   this.pageInfo.pageFilters = [];
        // } else {
        //   this.pageInfo.pageFilters = res;
        // }
        // if (callback) {
        //   callback();
        // }
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
        // if (!res || !res) {
        //   this.pageInfo.columns = [];
        // } else {
        //   this.pageInfo.columns = res;
        // }
        // if (callback) {
        //   callback();
        // }
      });
  }
}
