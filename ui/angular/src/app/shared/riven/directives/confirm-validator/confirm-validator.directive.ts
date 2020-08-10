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

  private _default = {};
  private _enabled = false;
  private _onChange?: () => void;
  private _confirmValue: string;

  constructor() {
  }

  @Input()
  set confirm(value: string) {
    this._confirmValue = value;
    this._enabled = typeof (value) === 'string';
    if (this._onChange) { this._onChange(); }
  }

  validate(control: AbstractControl): ValidationErrors | null {
    if (!this._enabled) {
      return this._default;
    }
    if (!control.value || control.value.trim() === '') {
      return this._default;
    }
    if (control.value !== this._confirmValue) {
      return { confirm: true, error: true };
    }

    return this._default;
  }

  registerOnValidatorChange(fn: () => void): void {
    this._onChange = fn;
  }
}
