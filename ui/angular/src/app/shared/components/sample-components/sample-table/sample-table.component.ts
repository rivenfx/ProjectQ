import {
  AfterViewInit,
  ChangeDetectionStrategy, ChangeDetectorRef,
  Component,
  Injector,
  Input, OnChanges,
  OnDestroy,
  OnInit, SimpleChange, SimpleChanges,
  ViewChild,
} from '@angular/core';
import { STColumn, STComponent, STPage } from '@delon/abc/st';
import { AppComponentBase } from '@shared/common';
import * as _ from 'lodash';
import { Subject } from 'rxjs';
import { SampleTableDataProcessorService } from '../sample-table-data-processor.service';
import { ISampleTableInfo } from './interfaces';

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

  /** 当action被点击 */
  onActionClick(action: string, record: any) {
    debugger;
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
