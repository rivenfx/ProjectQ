import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { IErrorDef } from './interfaces';
import { FormControl } from '@angular/forms';
import standartErrors from './standart-errors';


@Component({
  selector: 'validation-messages',
  templateUrl: './validation-messages.component.html',
  styleUrls: ['./validation-messages.component.less'],
})
export class ValidationMessagesComponent
  implements OnChanges {

  /** 表单控件 */
  @Input() formCtrl: FormControl;

  /** 自定义错误 */
  @Input() customErrors: IErrorDef[];

  /** 所有的错误 */
  errorDefsInternal: IErrorDef[] = standartErrors;


  constructor() {
  }

  ngOnChanges(changes: { [P in keyof this]?: import('@angular/core').SimpleChange } & import('@angular/core').SimpleChanges) {
    if (changes.customErrors) {
      this.updateErrorDefsInternal();
    }
  }

  updateErrorDefsInternal() {
    if (!this.customErrors || this.customErrors.length === 0) {
      return standartErrors;
    }
    const standarts = standartErrors.filter(stdErr => !this.customErrors.every(customErr => customErr.error === stdErr.error));
    this.errorDefsInternal = standarts.concat(this.customErrors);
  }


  getErrorDefinitionIsInValid(errorDef: IErrorDef): boolean {
    return !!this.formCtrl.errors[errorDef.error];
  }

  getErrorDefinitionMessage(errorDef: IErrorDef): string {
    let errorRequirement = this.formCtrl.errors[errorDef.error][errorDef.errorProperty];
    return !!errorRequirement ? errorDef.localizationKey + errorRequirement : errorDef.localizationKey;
    // return !!errorRequirement
    //   ? this.appLocalizationService.l(errorDef.localizationKey, errorRequirement)
    //   : this.appLocalizationService.l(errorDef.localizationKey);
  }
}
