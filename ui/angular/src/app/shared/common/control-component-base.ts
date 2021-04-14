import {
  AfterViewInit,
  ChangeDetectorRef,
  EventEmitter,
  Injector,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChange,
  SimpleChanges,
  Directive,
} from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';
import { SampleControlComponentBase } from './sample-control-component-base';

/***
 * 表单控件基类
 */
export abstract class ControlComponentBase<T> extends SampleControlComponentBase<T> implements OnChanges, ControlValueAccessor {
  /** 控件名称 */
  @Input()
  name: string;

  constructor(injector: Injector) {
    super(injector);

    this.cdr = injector.get(ChangeDetectorRef);
  }

  ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {
    if (changes.value) {
      this.writeValue(changes.value.currentValue);
    }
    this.onInputChange(changes);
  }

  registerOnChange(fn: any): void {
    this.valueChange.emit = fn;
  }

  registerOnTouched(fn: any): void {}

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  writeValue(obj: any): void {
    this.value = obj;
  }
}
