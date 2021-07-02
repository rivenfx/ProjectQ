import {
  ChangeDetectionStrategy, ChangeDetectorRef,
  Component,
  forwardRef,
  Injector,
  OnInit,
  SimpleChange,
  SimpleChanges,
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { RoleDto, RoleServiceProxy } from '@service-proxies';
import { ControlComponentBase } from '@rivenfx/ng-common';
import { finalize } from 'rxjs/operators';

interface ICheckBox<TValue> {
  label: string;
  value: TValue;
  tips: string;
  checked: boolean;
}

@Component({
  selector: 'role-select',
  templateUrl: './role-select.component.html',
  styleUrls: ['./role-select.component.less'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => RoleSelectComponent),
    multi: true,
  }],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoleSelectComponent extends ControlComponentBase<string[]> {

  roles: RoleDto[] = [];

  checkboxs: ICheckBox<string>[] = [];

  constructor(
    injector: Injector,
    private roleSer: RoleServiceProxy,
  ) {
    super(injector);
  }


  onAfterViewInit(): void {
  }

  onDestroy(): void {
  }

  onInit(): void {
    this.loading = true;
    this.roleSer.getAll()
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.roles = res.items;
        this.processDataToCheckBox();
      });
  }

  onInputChange(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges) {

  }
  writeValue(obj: any): void {
    super.writeValue(obj);
    this.processDataToCheckBox();
  }

  onCheckedChange(event: string[]) {
    this.value = event;
    this.emitValueChange(this.value);
  }

  protected processDataToCheckBox() {
    if (!Array.isArray(this.roles) || this.roles.length === 0) {
      return;
    }

    if (Array.isArray(this.value) && this.value.length > 0) {
      this.checkboxs = this.roles.map<ICheckBox<string>>(role => {
        return {
          label: role.displayName,
          value: role.name,
          tips: !!role.description ? role.description : role.displayName,
          checked: this.value.findIndex(item => item === role.name) !== -1,
        };
      });
    } else {
      this.checkboxs = this.roles.map<ICheckBox<string>>(role => {
        return {
          label: role.displayName,
          value: role.name,
          tips: !!role.description ? role.description : role.displayName,
          checked: false,
        };
      });
    }

    this.cdr.detectChanges();
  }
}
