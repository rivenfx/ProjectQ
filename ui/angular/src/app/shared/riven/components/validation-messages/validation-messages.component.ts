import {
  ChangeDetectionStrategy, ChangeDetectorRef,
  Component,
  Injector,
  Input,
  OnChanges,
  SimpleChange,
  SimpleChanges,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { SampleComponentBase } from '@shared/common';
import { IErrorDef } from './interfaces';
import standartErrors from './standart-errors';

@Component({
  selector: 'validation-messages',
  templateUrl: './validation-messages.component.html',
  styleUrls: ['./validation-messages.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ValidationMessagesComponent extends SampleComponentBase
  implements OnChanges {

  private _formCtrlInited = false;
  /** 表单控件 */
  @Input() formCtrl: FormControl;
  /** 自定义错误 */
  @Input() customErrors: IErrorDef[];
  /** 所有异常 */
  errorSource: any = {};
  /** 页面异常信息 */
  viewErrors: string[] = [];

  constructor(
    injector: Injector,
    private cdr: ChangeDetectorRef,
  ) {
    super(injector);

    this.updateErrorSource();
  }

  ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges) {
    if (changes.customErrors) {
      this.updateErrorSource();
    }
    if (changes.formCtrl && changes.formCtrl.currentValue && !this._formCtrlInited) {
      this._formCtrlInited = true;
      const self = this;
      self.updateViewError();
      changes.formCtrl.currentValue.valueChanges.subscribe((res) => {
        self.updateViewError();
      });
    }
  }

  /** 更新所有定义的错误 */
  private updateErrorSource() {
    if (!this.customErrors || this.customErrors.length === 0) {
      standartErrors.forEach((item) => {
        this.errorSource[item.error] = item;
      });
      return;
    }

    const standarts = standartErrors.filter(stdErr => !this.customErrors.every(customErr => customErr.error === stdErr.error));
    standarts.concat(this.customErrors)
      .forEach((item) => {
        this.errorSource[item.error] = item;
      });
  }

  /** 更新视图错误 */
  private updateViewError() {
    if (this.formCtrl.invalid && (this.formCtrl.dirty || this.formCtrl.touched)) {
      const tmpViewErrors = [];
      for (const key in this.formCtrl.errors) {
        const formError = this.formCtrl.errors[key];
        if (!formError) {
          continue;
        }
        const error: IErrorDef = this.errorSource[key];
        if (!error) {
          continue;
        }

        const errorRequirement = formError[error.errorProperty];
        tmpViewErrors.push(
          !!errorRequirement ? this.l(error.localizationKey) + ' ' + errorRequirement : this.l(error.localizationKey),
        );
      }

      const newError = tmpViewErrors.join(',');
      const oldError = this.viewErrors.join(',');
      if (newError !== oldError) {
        this.viewErrors = tmpViewErrors;
        this.cdr.detectChanges();
      }
    }
  }
}
