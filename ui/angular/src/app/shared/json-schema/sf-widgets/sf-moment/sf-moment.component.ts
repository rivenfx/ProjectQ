import { Component, OnInit } from '@angular/core';
import { ControlUIWidget, FormProperty, SFDateWidgetSchema, SFValue, toBool } from '@delon/form';
import { format } from 'date-fns';
import { toDate } from '@delon/util/date-time';
import { NzSafeAny } from 'ng-zorro-antd/core/types';
import * as moment from 'moment';
import { SFMomentWidgetSchema } from './schema';

@Component({
  selector: 'sf-moment',
  templateUrl: './sf-moment.component.html',
})
export class SfMomentComponent extends ControlUIWidget<SFMomentWidgetSchema> implements OnInit {
  /* 用于注册小部件 KEY 值 */
  static readonly KEY = 'moment';

  private startFormat: string;
  private endFormat: string;
  private flatRange = false;
  mode: string;
  displayValue: Date | Date[] | null = null;
  displayFormat: string;
  i: any;

  ngOnInit(): void {
    const { mode, end, displayFormat, allowClear, showToday } = this.ui;
    this.mode = mode || 'date';
    this.flatRange = end != null;
    // 构建属性对象时会对默认值进行校验，因此可以直接使用 format 作为格式化属性
    this.startFormat = this.ui._format!;
    if (this.flatRange) {
      this.mode = 'range';
      const endUi = this.endProperty.ui as SFDateWidgetSchema;
      this.endFormat = endUi.format ? endUi._format : this.startFormat;
    }
    if (!displayFormat) {
      switch (this.mode) {
        case 'year':
          this.displayFormat = `yyyy`;
          break;
        case 'month':
          this.displayFormat = `yyyy-MM`;
          break;
        case 'week':
          this.displayFormat = `yyyy-ww`;
          break;
      }
    } else {
      this.displayFormat = displayFormat;
    }
    this.i = {
      allowClear: toBool(allowClear, true),
      // nz-date-picker
      showToday: toBool(showToday, true),
    };
    if (!this.startFormat) {
      this.startFormat = 'yyyy-MM-dd';
    }
    if (!this.endFormat) {
      this.endFormat = 'yyyy-MM-dd';
    }
  }

  reset(value: SFValue): void {
    const toDateOptions = { formatString: this.startFormat, defaultValue: null };
    if (Array.isArray(value)) {
      value = value
        .map((o) => {
          if (moment.isMoment(o)) {
            return (o as moment.Moment).toDate();
          } else if (moment.isDate(o)) {
            return o;
          }
          return o;
        })
        .map((v) => toDate(v, toDateOptions));
    } else {
      let tmpVal: any;
      if (moment.isMoment(value)) {
        tmpVal = (value as moment.Moment).toDate();
      } else if (moment.isDate(value)) {
        tmpVal = value;
      }
      value = toDate(tmpVal, toDateOptions);
    }
    if (this.flatRange) {
      const endValue = toDate(this.endProperty.formData as NzSafeAny, {
        formatString: this.endFormat || this.startFormat,
        defaultValue: null,
      });
      this.displayValue = value == null || endValue == null ? [] : [value, endValue];
    } else {
      this.displayValue = value;
    }
    this.detectChanges();
    // TODO: Need to wait for the rendering to complete, otherwise it will be overwritten of end widget
    if (this.displayValue) {
      setTimeout(() => this._change(this.displayValue, false));
    }
  }

  _change(value: Date | Date[] | null, emitModelChange: boolean = true): void {
    if (emitModelChange && this.ui.change) {
      if (Array.isArray(value)) {
        this.ui.change(value.map((o) => moment(o)));
      } else {
        this.ui.change(!!value ? moment(value) : null);
      }
    }
    if (value == null || (Array.isArray(value) && value.length < 2)) {
      this.setValue(null);
      this.setEnd(null);
      return;
    }

    const res = Array.isArray(value)
      ? [format(value[0], this.startFormat), format(value[1], this.endFormat || this.startFormat)]
      : format(value, this.startFormat);

    if (this.flatRange) {
      this.setValue(moment(res[0]));
      this.setEnd(res[1]);
    } else {
      this.setValue(moment(res));
    }
  }

  _openChange(status: boolean): void {
    if (this.ui.onOpenChange) {
      this.ui.onOpenChange(status);
    }
  }

  _ok(value: Date | Date[] | null): void {
    if (this.ui.onOk) {
      if (!value) {
        this.ui.onOk(null);
        return;
      }
      if (!Array.isArray(value)) {
        this.ui.onOk(moment(value));
        return;
      }

      this.ui.onOk(value.map((o) => moment(o)));
    }
  }

  private get endProperty(): FormProperty {
    return (this.formProperty.parent!.properties as { [key: string]: FormProperty })[this.ui.end!];
  }

  private setEnd(value: string | null): void {
    if (!this.flatRange) {
      return;
    }

    this.endProperty.setValue(value, true);
    this.endProperty.updateValueAndValidity();
  }
}
