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
import { STChange, STColumn, STComponent, STMultiSort, STPage } from '@delon/abc/st';
import { ColumnItemDto, SortCondition, SortType } from '@service-proxies';
import { AppComponentBase } from '@shared/common';
import * as _ from 'lodash';
import { Subject } from 'rxjs';
import { SampleTableDataProcessorService } from '../sample-table-data-processor.service';
import { ISampleTableAction } from './interfaces';

@Component({
  selector: 'sample-table',
  templateUrl: './sample-table.component.html',
  styleUrls: ['./sample-table.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SampleTableComponent extends AppComponentBase
  implements OnInit, AfterViewInit, OnChanges, OnDestroy {


  /** 列表列配置 */
  @Input() columns: ColumnItemDto[] = [];

  /** 数据 */
  @Input() data: any[] = [];

  /** 列表配置 */
  @Input() page: STPage = {
    front: false,
    show: true,
    showQuickJumper: true,
    pageSizes: [10, 20, 30, 40, 50]
  };

  /** 页码 */
  @Input() pageIndex = 1;

  /** 页面数据量 */
  @Input() pageSize = 20;

  /** 页面数据分页条选项 */
  @Input() pageSizes = [10, 20, 30, 40, 50];

  /** 数据总量 */
  @Input() total: number;

  /** 边框 */
  @Input() bordered = true;

  /** 列被触发 */
  @Output() action = new EventEmitter<ISampleTableAction>();

  /** 列表排序事件 */
  @Output() sort = new EventEmitter<SortCondition[]>();

  /** 当check多选 */
  @Output() check = new EventEmitter<any[]>();

  /** 页面数据大小发生改变 */
  @Output() pageSizeChange = new EventEmitter<number>();

  /** 页码发生改变 */
  @Output() pageIndexChange = new EventEmitter<number>();

  /** 列表数据 */
  tableData: any = [];

  /** 列表配置 */
  tableColumns: STColumn[] = [];

  /** 排序配置 */
  sortData: STMultiSort = {
    key: 'sort',
    separator: '-',
    nameSeparator: '.',
    keepEmptyKey: true,
    global: true
  };

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
    if (changes.columns && changes.columns.currentValue) {
      this.processColumns(changes.columns.currentValue);
    }
    if (changes.data && changes.data.currentValue) {
      this.processDatas(changes.data.currentValue);
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /** 当 action 项被点击 */
  onActionClick(action: string, record: any) {
    this.action.emit({
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
        this.check.emit(checked);
        break;
      case 'radio': // 单选
        this.check.emit([evnet.radio]);
        break;
      case 'sort': // 排序
        this.updateSort(evnet);
        break;
      case 'pi': // page index 修改
        this.pageIndexChange.emit(evnet.pi);
        break;
      case 'ps': // page size 修改
        this.pageSizeChange.emit(evnet.ps);
        break;
    }
  }

  /** 更新排序 */
  protected updateSort(evnet: STChange) {
    debugger;
    if (evnet.type !== 'sort') {
      return;
    }

    const sortConditions: SortCondition[] = [];
    const sorts = evnet.sort.map.sort.split('-');
    let sortField;
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

    this.sort.emit(sortConditions);
  }

  /** 处理列表信息 */
  protected processColumns(input: ColumnItemDto[]) {
    this.tableColumns = [];
    if (!input || input.length === 0) {
      this.cdr.detectChanges();
      return;
    }

    this.tableColumns = this.tableDataProcessor.processCols(input);
    this.cdr.detectChanges();
  }

  /*** 处理数据 */
  protected processDatas(input: any[]) {
    this.tableData = [];
    if (!input || input.length === 0) {
      this.cdr.detectChanges();
      return;
    }
    this.tableData = this.tableDataProcessor.processData<any>(input, this.columns);
    this.cdr.detectChanges();
  }
}
