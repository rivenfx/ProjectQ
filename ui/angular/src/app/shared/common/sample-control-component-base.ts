import { AfterViewInit, ChangeDetectorRef, EventEmitter, Injector, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChange, SimpleChanges, Directive } from '@angular/core';
import { SampleComponentBase } from './sample-component-base';

/***
 * 简单控件基类
 */
@Directive()
export abstract class SampleControlComponentBase<T> extends SampleComponentBase
  implements OnInit, AfterViewInit, OnDestroy, OnChanges {

  /** 占位符 */
  @Input()
  placeholder = '';

  /** 启用清除,默认为false */
  @Input()
  enabledClear: boolean;

  /** 启用过滤,默认为false */
  @Input()
  enabledFileter: boolean;

  /** 禁用,默认为false */
  @Input()
  disabled: boolean;

  /** 值 */
  @Input()
  value: T;

  /** 值发生更改事件 */
  @Output()
  valueChange = new EventEmitter<T>();

  cdr: ChangeDetectorRef;

  constructor(injector: Injector) {
    super(injector);

    this.cdr = injector.get(ChangeDetectorRef);
  }

  ngOnInit(): void {
    this.onInit();
  }

  ngAfterViewInit(): void {
    this.onAfterViewInit();
  }

  ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {
    this.onInputChange(changes);
  }

  ngOnDestroy(): void {
    this.onDestroy();
  }

  emitValueChange(val: T) {
    this.value = val;
    this.valueChange.emit(val);
  }

  /** 初始化 */
  abstract onInit(): void;

  /** 视图初始化完成 */
  abstract onAfterViewInit(): void;

  /** @Input()标记的值发生改变 */
  abstract onInputChange(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges);

  /** 释放资源 */
  abstract onDestroy(): void;
}
