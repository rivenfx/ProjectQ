import {
  ChangeDetectionStrategy, ChangeDetectorRef,
  Component,
  Injector,
  Input, OnChanges,
  OnInit,
  SimpleChange,
  SimpleChanges,
} from '@angular/core';
import { PageFilterItemDto, PageFilterServiceProxy, QueryCondition } from '@service-proxies';
import { SampleControlComponentBase } from '@shared/common';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'page-filter',
  templateUrl: './page-filter.component.html',
  styleUrls: ['./page-filter.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageFilterComponent extends SampleControlComponentBase<QueryCondition[]> {


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
  pageFilterData: { [P in string]: QueryCondition } = {};

  /** 依赖的输入数据 */
  pageFilterExternalArgsData: any = {};
  /** 存在基本搜索 */
  existBasicFilter: boolean;
  /** 存在高级搜索 */
  existAdvancedFilter: boolean;
  /** 是否展开高级搜索 */
  isCollapsed = true;

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

  onClickCollapse() {
    this.isCollapsed = !this.isCollapsed;
    this.cdr.detectChanges();
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
        externalArgs[item.field] = event;
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
    this.existBasicFilter = false;
    this.existAdvancedFilter = false;
    this.isCollapsed = true;

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
      this.pageFilterData[item.field] = new QueryCondition({
        field: item.field,
        operator: item.operator,
        value: undefined,
        skipValueIsNull: !!item.skipValueIsNull,
      });
      this.pageFilterExternalArgsData[item.field] = undefined;
    });

    this.basicFilters = enabledPageFilters.filter(o => !o.advanced);
    this.advancedFilters = enabledPageFilters.filter(o => o.advanced);
    if (this.basicFilters.length > 0) {
      this.existBasicFilter = true;
    }
    if (this.advancedFilters.length > 0) {
      this.existAdvancedFilter = true;
    }

    this.cdr.detectChanges();
  }


}
