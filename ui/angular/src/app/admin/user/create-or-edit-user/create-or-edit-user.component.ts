import { Component, Injector, OnInit } from '@angular/core';
import { CreateOrEditUserInput, CreateOrUpdateRoleInput, UserDto, UserServiceProxy } from '@service-proxies';
import { AppConsts } from '@shared';
import { ModalComponentBase } from '@shared/common';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'create-or-edit-user',
  templateUrl: './create-or-edit-user.component.html',
  styleUrls: ['./create-or-edit-user.component.less'],
})
export class CreateOrEditUserComponent extends ModalComponentBase<string>
  implements OnInit {

  user = new UserDto();
  roles: string[] = [];

  password: string;
  passwordConfimd: string;

  constructor(
    injector: Injector,
    private userSer: UserServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.title = this.l('menu.user');
    if (this.modalInput) {
      this.loading = true;
      this.userSer.getEditById(this.modalInput)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe((res) => {
          if (!res) {
            return;
          }
          if (res.entityDto) {
            this.user = res.entityDto;
          }

          if (Array.isArray(res.roles)) {
            this.roles = res.roles;
          }

          if (!this.readonly) {
            this.readonly = this.user.isStatic;
          }
          this.disableFormControls();
        });
    }
  }

  submitForm(event?: any) {
    const input = new CreateOrEditUserInput({
      entityDto: this.user,
      password: this.password,
      roles: this.roles,
    });

    this.loading = true;
    if (this.modalInput) {
      this.userSer.update(input)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(() => {
          this.message.success(this.l(AppConsts.message.success));
          this.success();
        });
    } else {
      this.userSer.create(input)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(() => {
          this.message.success(this.l(AppConsts.message.success));
          this.success();
        });
    }
  }
}
