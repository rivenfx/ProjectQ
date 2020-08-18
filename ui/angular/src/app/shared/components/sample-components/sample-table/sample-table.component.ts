import {
  AfterViewInit,
  ChangeDetectionStrategy, ChangeDetectorRef,
  Component,
  EventEmitter,
  Injector, Input,
  OnChanges,
  OnDestroy, OnInit, Output,
  SimpleChange,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { STColumn, STComponent, STPage, STChange } from '@delon/abc/st';
import { AppComponentBase } from '@shared/common';
import * as _ from 'lodash';
import { Subject } from 'rxjs';
import { SampleTableDataProcessorService } from '../sample-table-data-processor.service';
import { ISampleTableAction, ISampleTableInfo } from './interfaces';
import { SortCondition, SortType } from '@service-proxies';

@Component({
  selector: 'sample-table',
  templateUrl: './sample-table.component.html',
  styleUrls: ['./sample-table.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SampleTableComponent extends AppComponentBase
  implements OnInit, AfterViewInit, OnChanges, OnDestroy {


  /** 列表信息 */
  @Input() info: ISampleTableInfo;

  /** 列表配置 */
  @Input() page: STPage = {
    front: false,
    show: true,
  };

  /** 数据总量 */
  @Input() total: number;

  /** 边框 */
  @Input() bordered = true;

  /** 列被触发 */
    // tslint:disable-next-line: no-output-on-prefix
  @Output() onAction = new EventEmitter<ISampleTableAction>();

  /** 列表排序事件 */
  @Output() onSort = new EventEmitter<SortCondition[]>();

  /** 当check多选 */
  @Output() onCheck = new EventEmitter<any[]>();

  /** 页面数据大小发生改变 */
  @Output() onPageSizeChange = new EventEmitter<number>();

  /** 页码发生改变 */
  @Output() onPageIndexChange = new EventEmitter<number>();

  /** 列表数据 */
  tableData: any = [];

  /** 列表配置 */
  tableColumns: STColumn[] = [];

  private destroy$ = new Subject();
  @ViewChild('st', { static: false }) stRef: STComponent;

  constructor(
    injector: Injector,
    private cdr: ChangeDetectorRef,
    private tableDataProcessor: SampleTableDataProcessorService,
  ) {
    super(injector);
  }

  ngOnInit(): void {

  }

  ngAfterViewInit(): void {

  }

  ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {
    if (changes.info && changes.info.currentValue) {
      this.processTableInfo(changes.info.currentValue);
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /** 当 action 项被点击 */
  onActionClick(action: string, record: any) {
    this.onAction.emit({
      name: action,
      // tslint:disable-next-line: object-literal-shorthand
      record: record,
    });
  }

  /** 表格事件 */
  onTableChange(evnet: STChange) {
    switch (evnet.type) {
      case 'checkbox': // 多选
        const checked = evnet.checkbox;
        this.onCheck.emit(checked);
        break;
      case 'radio': // 单选
        this.onCheck.emit([evnet.radio]);
        break;
      case 'sort': // 排序
        this.updateSort(evnet);
        break;
      case 'pi': // page index 修改
        this.onPageIndexChange.emit(evnet.pi);
        break;
      case 'ps': // page size 修改
        this.onPageSizeChange.emit(evnet.ps);
        break;
    }
  }

  /** 更新排序 */
  protected updateSort(evnet: STChange) {
    if (evnet.type !== 'sort') {
      return;
    }

    const sortConditions: SortCondition[] = [];
    const sorts = evnet.sort.map.sort.split('-');
    let sortField = undefined;
    let sortType: SortType = SortType.None;
    let index = 0;
    for (const sort of sorts) {
      const lastIndex = sort.lastIndexOf('.');
      sortField = sort.substring(0, lastIndex);
      if (sort.endsWith('.descend')) {
        sortType = SortType.Desc;
      } else if (sort.endsWith('.ascend')) {
        sortType = SortType.Asc;
      }
      sortConditions.push(
        new SortCondition({
          field: sortField,
          order: index++,
          type: sortType,
        }),
      );
    }

    this.onSort.emit(sortConditions);
  }

  /** 处理列表信息 */
  protected processTableInfo(input: ISampleTableInfo) {
    this.tableData = [];
    this.tableColumns = [];
    if (!input) {
      this.cdr.detectChanges();
      return;
    }

    this.tableData = this.tableDataProcessor.processData<any>(input.data, input.columns);
    this.tableColumns = this.tableDataProcessor.processCols(input.columns);

    if (typeof (input.displayPagination) === 'boolean') {
      debugger;
      const tmpPage = _.clone(this.page);
      tmpPage.show = input.displayPagination;
      this.page = tmpPage;
    }

    this.cdr.detectChanges();
  }
}
