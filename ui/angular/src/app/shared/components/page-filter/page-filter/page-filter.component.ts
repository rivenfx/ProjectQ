import {
  ChangeDetectionStrategy, ChangeDetectorRef,
  Component,
  EventEmitter,
  Injector, Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChange,
  SimpleChanges,
} from '@angular/core';
import { PageFilterItemDto, QueryCondition } from '@service-proxies';
import { SampleControlComponentBase } from '@shared/common';
import * as _ from 'lodash';

@Component({
  selector: 'page-filter',
  templateUrl: './page-filter.component.html',
  styleUrls: ['./page-filter.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageFilterComponent extends SampleControlComponentBase<QueryCondition[]> {

  /** 筛选条件 */
  @Input() pageFilters: PageFilterItemDto[] = [];

  /** 显示标签 */
  @Input() displayLabel: boolean;

  /** 基本筛选条件显示列,超出的将属于高级筛选条件 */
  @Input() basicRow = 1;

  /** 控件加载完成 */
  @Output() readyChange = new EventEmitter<QueryCondition[]>();

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

  /** 已经准备好的组件 */
  private _readyName: string[] = [];
  /*** 启用的filter数量 */
  private _enabledFilterCount = 0;

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  onInit(): void {

  }

  onAfterViewInit(): void {

  }

  onInputChange(changes: { [P in keyof this]?: SimpleChange; } & SimpleChanges) {
    if (changes.pageFilters) {
      if (!Array.isArray(changes.pageFilters.currentValue)) {
        this.pageFilters = [];
      }
      this.processFilters();
    }
  }

  onDestroy(): void {

  }


  /** 当控件准备好 */
  onReadyChange(name: string) {
    this._readyName.push(name);

    this._readyName = _.uniq(this._readyName);

    if (this._enabledFilterCount === this._readyName.length) {
      this.readyChange.emit(this.value);
    }
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
      if (this.pageFilterData[key].value === '') {
        continue;
      }
      tmpValue.push(this.pageFilterData[key]);
    }
    this.emitValueChange(tmpValue);
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
    this._enabledFilterCount = 0;

    if (typeof (this.basicRow) !== 'number' || this.basicRow <= 0) {
      this.basicRow = 1;
    }

    if (!Array.isArray(this.pageFilters) || this.pageFilters.length === 0) {
      this.cdr.detectChanges();
      return;
    }

    const tmpEnabledPageFilters = this.pageFilters.filter(o => o.enabled);
    if (tmpEnabledPageFilters.length === 0) {
      this.cdr.detectChanges();
      return;
    }

    // 启用的pageFilter选项
    const enabledPageFilters = _.cloneDeep(tmpEnabledPageFilters)
      .sort((a, b) => {
        return a.order - b.order;
      });

    // 屏幕大小编码
    const screenSizeCode = this.getCurrentScreenSizeCode();
    // 宽度字段
    const widthFiled = `${screenSizeCode}Width`;
    // 基本过滤选项最大col数量
    const basicMaxColCount = this.basicRow * 24;
    // 临时col数量
    let tmpColCount = 0;

    // 遍历可用的pageFilter
    for (const item of enabledPageFilters) {
      // 预处理数据,初始化映射
      if (item.width <= 0) {
        item.width = 6;
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
        skipValueIsNull: item.skipValueIsNull === true,
      });
      this.pageFilterExternalArgsData[item.field] = undefined;

      // 根据宽度动态计算是否为高级过滤选项
      tmpColCount += item[widthFiled];
      if (tmpColCount <= basicMaxColCount) {
        this.basicFilters.push(item);
      } else {
        this.advancedFilters.push(item);
      }
    }

    if (this.basicFilters.length > 0) {
      this.existBasicFilter = true;
    }
    if (this.advancedFilters.length > 0) {
      this.existAdvancedFilter = true;
    }

    this._enabledFilterCount = this.basicFilters.length + this.advancedFilters.length;

    this.cdr.detectChanges();
  }

  getCurrentScreenSizeCode(): string {
    const width = window.innerWidth;
    if (width < 575) {
      return 'xs';
    }
    if (width >= 575 && width < 768) {
      return 'sm';
    }
    if (width >= 768 && width < 992) {
      return 'md';
    }
    if (width >= 992 && width < 1200) {
      return 'lg';
    }
    if (width >= 1200 && width < 1600) {
      return 'xl';
    }
    if (width >= 1600) {
      return 'xxl';
    }

    return 'md';
  }


}
