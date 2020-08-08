import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator } from '@angular/forms';

@Directive({
  selector: ':not([type=checkbox])[confirm][formControlName],:not([type=checkbox])[confirm][formControl],:not([type=checkbox])[confirm][ngModel]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => ConfirmValidatorDirective),
      multi: true,
    },
  ],
})
export class ConfirmValidatorDirective implements Validator {

  private _enabled = false;
  private _onChange?: () => void;
  private _confirmValue: string;

  constructor() {
  }

  /**
   * @description
   * Tracks changes to the email attribute bound to this directive.
   */
  @Input()
  set confirm(value: string) {
    this._confirmValue = value;
    this._enabled = !!value;
    if (this._onChange) this._onChange();
  }

  validate(control: AbstractControl): ValidationErrors | null {
    if (!this._enabled) {
      return {};
    }
    if (control.value !== this._confirmValue) {
      return { confirm: true, error: true };
    }

    return {};
  }

  registerOnValidatorChange(fn: () => void): void {
    this._onChange = fn;
  }
}
