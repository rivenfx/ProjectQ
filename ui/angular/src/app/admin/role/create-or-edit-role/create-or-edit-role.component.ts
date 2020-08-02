import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase, ModalComponentBase } from '@shared/common';
import { CreateOrUpdateRoleInput, RoleDto, RoleServiceProxy } from '@service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'create-or-edit-role',
  templateUrl: './create-or-edit-role.component.html',
  styleUrls: ['./create-or-edit-role.component.less'],
})
export class CreateOrEditRoleComponent extends ModalComponentBase<string>
  implements OnInit {

  role = new RoleDto();
  claims: string[] = [];

  constructor(
    injector: Injector,
    public roleSer: RoleServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if (this.modalInput) {
      this.loading = true;
      this.roleSer.getRoleById(this.modalInput)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe((res) => {
          this.role = res.entityDto;
          this.claims = res.claims;
        });
    }
  }

  submitForm(event?: any) {
    const input = new CreateOrUpdateRoleInput({
      entityDto: this.role,
      claims: this.claims,
    });

    if (this.modalInput) {
      this.roleSer.update(input)
        .subscribe(() => {
          this.message.success(this.l('message.success.update'));
          this.success();
        });
    } else {
      this.roleSer.create(input)
        .subscribe(() => {
          this.message.success(this.l('message.success.create'));
          this.success();
        });
    }

  }
}
