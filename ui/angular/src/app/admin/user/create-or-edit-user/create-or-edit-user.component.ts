import { Component, Injector, OnInit } from '@angular/core';
import { CreateOrEditUserInput, CreateOrUpdateRoleInput, UserDto, UserServiceProxy } from '@service-proxies';
import { AppConsts } from '@shared';
import { ModalComponentBase } from '@shared/common';
import { finalize } from 'rxjs/operators';
import { SFSchema } from '@delon/form';

@Component({
  selector: 'create-or-edit-user',
  templateUrl: './create-or-edit-user.component.html',
  styleUrls: ['./create-or-edit-user.component.less']
})
export class CreateOrEditUserComponent extends ModalComponentBase<string>
  implements OnInit {

  user = new UserDto();
  roles: string[] = [];

  password: string;
  passwordConfimd: string;

  schema: SFSchema = {
    properties: {
      userName: {
        type: 'string',
        title: '账号',
        minLength: 5
      },
      nickname: {
        type: 'string',
        title: '昵称',
        minLength: 5
      },
      password: {
        title: '密码',
        type: 'string',
        minLength: 5
      },
      passwordConfimd: {
        title: '确认密码',
        type: 'string',
        minLength: 5
      },
      phoneNumber: {
        type: 'string',
        title: '电话号码'
      },
      email: {
        type: 'string',
        title: '邮箱',
        format: 'email'
      },
      phoneNumberConfirmed: {
        title: '电话号码确认',
        type: 'boolean',
        ui: {
          grid: {
            span: 6
          }
        }
      },
      emailConfirmed: {
        title: '邮箱确认',
        type: 'boolean',
        ui: {
          grid: {
            span: 6
          }
        }
      },
      lockoutEnabled: {
        title: '登录锁定',
        type: 'boolean',
        ui: {
          grid: {
            span: 6
          }
        }

      },
      isActive: {
        title: '激活',
        type: 'boolean',
        ui: {
          grid: {
            span: 6
          }
        }
      }
    },
    required: ['email', 'name'],
    ui: {
      errors: {
        minLength: this.l('validation.minlength'),
        maxLength: this.l('validation.maxlength'),
        required: this.l('validation.required')
      },
      spanLabelFixed: 100,
      grid: {
        span: 12
      }
    }
  };

  constructor(
    injector: Injector,
    private userSer: UserServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.title = this.l('user');
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
      roles: this.roles
    });

    this.loading = true;
    if (this.modalInput) {
      this.userSer.update(input)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(() => {
          this.notify.success(this.l(AppConsts.message.success));
          this.success();
        });
    } else {
      this.userSer.create(input)
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
