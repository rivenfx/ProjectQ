import { Component, Injector, OnInit } from '@angular/core';
import { CreateOrUpdateRoleInput, RoleDto, RoleServiceProxy } from '@service-proxies';
import { AppConsts } from '@shared';
import { AppComponentBase, ModalComponentBase } from '@shared/common';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'create-or-edit-role',
  templateUrl: './create-or-edit-role.component.html',
  styleUrls: ['./create-or-edit-role.component.less'],
})
export class CreateOrEditRoleComponent extends ModalComponentBase<string>
  implements OnInit {

  role = new RoleDto();
  permissions: string[] = [];

  constructor(
    injector: Injector,
    public roleSer: RoleServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.title = this.l('role');
    if (this.modalInput) {
      this.loading = true;
      this.roleSer.getEditById(this.modalInput)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe((res) => {
          this.role = res.entityDto;
          this.permissions = res.permissions;

          if (!this.readonly) {
            this.readonly = this.role.isStatic;
          }

          this.disableFormControls();
        });
    }
  }

  submitForm(event?: any) {
    const input = new CreateOrUpdateRoleInput({
      entityDto: this.role,
      permissions: this.permissions,
    });

    this.loading = true;
    if (this.modalInput) {
      this.roleSer.update(input)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(() => {
          this.notify.success(this.l(AppConsts.message.success));
          this.success();
        });
    } else {
      this.roleSer.create(input)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(() => {
          this.notify.success(this.l(AppConsts.message.success));
          this.success();
        });
    }

  }
}
