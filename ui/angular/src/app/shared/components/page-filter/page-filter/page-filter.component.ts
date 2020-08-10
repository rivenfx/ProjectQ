import {
  ChangeDetectionStrategy, ChangeDetectorRef,
  Component,
  Injector,
  Input, OnChanges,
  OnInit,
  SimpleChange,
  SimpleChanges,
} from '@angular/core';
import { PageFilterItemDto, PageFilterServiceProxy } from '@service-proxies';
import { SampleControlComponentBase } from '@shared/common';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';
import { IPageFilterItemData } from './interfaces';

@Component({
  selector: 'page-filter',
  templateUrl: './page-filter.component.html',
  styleUrls: ['./page-filter.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageFilterComponent extends SampleControlComponentBase<IPageFilterItemData[]> {


  /** 筛选条件配置文件名称 */
  @Input() pageFilterName: string;

  /** 筛选条件 */
  @Input() pageFilters: PageFilterItemDto[] = [];

  /** 显示标签 */
  @Input() displayLabel: boolean;

  /** 基本筛选条件 */
  basicFilters: PageFilterItemDto[] = [];

  /** 高级筛选条件 */
  advancedFilters: PageFilterItemDto[] = [];

  /** 筛选条件的数据 */
  pageFilterData: { [P in string]: IPageFilterItemData } = {};

  /** 依赖的输入数据 */
  pageFilterExternalArgsData: any = {};

  constructor(
    injector: Injector,
    private pageFilterSer: PageFilterServiceProxy,
  ) {
    super(injector);
  }

  onInit(): void {

  }
  onAfterViewInit(): void {

  }
  onInputChange(changes: { [P in keyof this]?: SimpleChange; } & SimpleChanges) {
    if (changes.pageFilterName && changes.pageFilterName.currentValue && changes.pageFilterName.currentValue.trim() !== '') {
      this.fetchData();
    }
    if (changes.pageFilters) {
      if (!Array.isArray(changes.pageFilters.currentValue)) {
        this.pageFilters = [];
      }
      this.processFilters();
    }
  }

  onDestroy(): void {

  }


  /** page-filter-item 组件数据发生改变 */
  onValueChange(event: any, item: PageFilterItemDto) {
    if (item.valueChange) {
      // 更新触发的组件的数据
      for (const key of item.valueChange) {
        let externalArgs = this.pageFilterExternalArgsData[key];
        if (!externalArgs) {
          externalArgs = {};
        }
        externalArgs[item.name] = event;
        this.pageFilterExternalArgsData[key] = _.clone(externalArgs);
      }
    }

    const tmpValue = [];
    // tslint:disable-next-line: forin
    for (const key in this.pageFilterData) {
      tmpValue.push(this.pageFilterData[key]);
    }
    this.emitValueChange(tmpValue);
  }

  /** 查询配置 */
  protected fetchData() {
    this.pageFilterSer.getPageFilter(this.pageFilterName)
      .pipe(finalize(() => {

      }))
      .subscribe((res) => {
        this.pageFilters = res.items;

        this.processFilters();
      });
  }

  /** 处理page-filter配置 */
  protected processFilters() {
    this.pageFilterData = {};
    this.pageFilterExternalArgsData = {};
    this.basicFilters = [];
    this.advancedFilters = [];

    if (!Array.isArray(this.pageFilters) || this.pageFilters.length === 0) {
      this.cdr.detectChanges();
      return;
    }

    const tmpEnabledPageFilters = this.pageFilters.filter(o => o.enabled);
    if (tmpEnabledPageFilters.length === 0) {
      this.cdr.detectChanges();
      return;
    }

    const enabledPageFilters = _.cloneDeep(tmpEnabledPageFilters);

    enabledPageFilters.forEach(item => {
      if (item.width <= 0) {
        item.width = 8;
      }
      if (typeof (item.xsWidth) !== 'number' || item.xsWidth <= 0) {
        item.xsWidth = item.width;
      }
      if (typeof (item.smWidth) !== 'number' || item.smWidth <= 0) {
        item.smWidth = item.width;
      }
      if (typeof (item.mdWidth) !== 'number' || item.mdWidth <= 0) {
        item.mdWidth = item.width;
      }
      if (typeof (item.lgWidth) !== 'number' || item.lgWidth <= 0) {
        item.lgWidth = item.width;
      }
      if (typeof (item.xlWidth) !== 'number' || item.xlWidth <= 0) {
        item.xlWidth = item.width;
      }
      if (typeof (item.xxlWidth) !== 'number' || item.xxlWidth <= 0) {
        item.xxlWidth = item.width;
      }
      if (!Array.isArray(item.valueChange)) {
        item.valueChange = undefined;
      } else if (item.valueChange.length === 0) {
        item.valueChange = undefined;
      }
      this.pageFilterData[item.name] = {
        name: item.name,
        condition: item.condition,
        value: undefined,
      };
      this.pageFilterExternalArgsData[item.name] = undefined;
    });

    this.basicFilters = enabledPageFilters.filter(o => !o.advanced);
    this.advancedFilters = enabledPageFilters.filter(o => o.advanced);

    this.cdr.detectChanges();
  }


}
