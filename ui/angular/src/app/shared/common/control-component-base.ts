import {
  AfterViewInit,
  EventEmitter,
  Injector,
  Input,
  OnChanges, OnDestroy,
  OnInit,
  Output,
  SimpleChange,
  SimpleChanges,
} from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';
import { SampleComponentBase } from './sample-component-base';

/***
 * 控件基类
 */
export abstract class AppControlComponentBase<T> extends SampleComponentBase
  implements OnInit, AfterViewInit, OnDestroy, ControlValueAccessor, OnChanges {

  /** 控件名称 */
  @Input()
  name: string;

  /** 占位符 */
  @Input()
  placeholder: string;

  /** 启用清除,默认为false */
  @Input()
  showClear: boolean;

  /** 启用过滤,默认为false */
  @Input()
  enableFileter: boolean;

  /** 禁用,默认为false */
  @Input()
  disabled: boolean;

  /** 只读，默认为false */
  @Input()
  readonly: boolean;

  /** 值 */
  @Input()
  value: T;

  /** 值发生更改事件 */
  @Output()
  valueChange = new EventEmitter<T>();


  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
    this.onInit();
  }

  ngAfterViewInit(): void {
    this.onAfterViewInit();
  }

  ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {
    if (changes.value) {
      this.writeValue(changes.value.currentValue);
    }
    this.onInputChange(changes);
  }

  ngOnDestroy(): void {
    this.onDestroy();
  }

  registerOnChange(fn: any): void {
    this.valueChange.emit = fn;
  }

  registerOnTouched(fn: any): void {
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  writeValue(obj: any): void {
    this.value = obj;
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
