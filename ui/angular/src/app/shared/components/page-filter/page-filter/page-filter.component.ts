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
import { SampleComponentBase } from '@shared/common';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'page-filter',
  templateUrl: './page-filter.component.html',
  styleUrls: ['./page-filter.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageFilterComponent extends SampleComponentBase
  implements OnInit, OnChanges {

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
  pageFilterData: any = {};

  /** 依赖的输入数据 */
  pageFilterExternalArgsData: any = {};

  constructor(
    injector: Injector,
    private cdr: ChangeDetectorRef,
    private pageFilterSer: PageFilterServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
  }

  ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {
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

  onValueChange(event: any, item: PageFilterItemDto) {
    if (!item.valueChange) {
      return;
    }

    // 更新触发的组件的数据
    for (let i = 0; i < item.valueChange.length; i++) {
      const key = item.valueChange[i];
      let externalArgs = this.pageFilterExternalArgsData[key];
      if (!externalArgs) {
        externalArgs = {};
      }
      externalArgs[item.name] = event;
      this.pageFilterExternalArgsData[key] = _.clone(externalArgs);
    }
  }

  protected fetchData() {
    this.pageFilterSer.getPageFilter(this.pageFilterName)
      .pipe(finalize(() => {

      }))
      .subscribe((res) => {
        this.pageFilters = res.items;

        this.processFilters();
      });
  }


  protected processFilters() {
    debugger;
    this.pageFilterData = {};
    this.pageFilterExternalArgsData = {};
    this.basicFilters = [];
    this.advancedFilters = [];

    if (!Array.isArray(this.pageFilters) || this.pageFilters.length === 0) {
      this.cdr.detectChanges();
      return;
    }

    // todo: 需要深度克隆 pageFilters

    const enabledPageFilters = this.pageFilters.filter(o => o.enabled);
    if (enabledPageFilters.length === 0) {
      this.cdr.detectChanges();
      return;
    }

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
      this.pageFilterData[item.name] = undefined;
      this.pageFilterExternalArgsData[item.name] = undefined;
    });

    this.basicFilters = enabledPageFilters.filter(o => !o.advanced);
    this.advancedFilters = enabledPageFilters.filter(o => o.advanced);

    this.cdr.detectChanges();
  }


}
