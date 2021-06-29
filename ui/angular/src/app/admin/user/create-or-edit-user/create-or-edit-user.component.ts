import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CreateOrEditUserInput, CreateOrUpdateRoleInput, UserDto, UserServiceProxy } from '@service-proxies';
import { AppConsts } from '@shared';
import { ModalComponentBase } from '@shared/common';
import { finalize } from 'rxjs/operators';
import { SFComponent, SFSchema } from '@delon/form';

@Component({
  selector: 'create-or-edit-user',
  templateUrl: './create-or-edit-user.component.html',
  styleUrls: ['./create-or-edit-user.component.less'],
})
export class CreateOrEditUserComponent extends ModalComponentBase<string>
  implements OnInit {

  @ViewChild('roleForm', { static: false }) roleFormRef: SFComponent;

  user = new UserDto();

  role = {
    roles: []
  };

  /** 基本表单配置 */
  pageFormSchema: SFSchema = {
    properties: {
      userName: {
        type: 'string',
        title: this.l('user.user-name'),
        minLength: 5,
      },
      nickname: {
        title: this.l('user.nick-name'),
        type: 'string',
        minLength: 5,
      },
      password: {
        title: this.l('label.password'),
        type: 'string',
        minLength: 5,
        maxLength: 32,
        ui: {
          type: 'password',
        }
      },
      passwordConfimd: {
        title: this.l('label.password-confirm'),
        type: 'string',
        minLength: 5,
        maxLength: 32,
        ui: {
          type: 'password',
          validator: (val, fp, f) => {
            if (val !== f.getProperty('password').value) {
              return [{ keyword: 'confirm', message: this.l('validation.confirm') }];
            }
            return [];
          }
        }
      },
      phoneNumber: {
        title: this.l('user.phone-number'),
        type: 'string',
      },
      email: {
        title: this.l('user.email'),
        type: 'string',
        format: 'email',
      },
      phoneNumberConfirmed: {
        title: this.l('user.phone-number-confirmed'),
        type: 'boolean',
        ui: {
          grid: {
            span: 6,
          },
        },
      },
      emailConfirmed: {
        title: this.l('user.email-confirmed'),
        type: 'boolean',
        ui: {
          grid: {
            span: 6,
          },
        },
      },
      lockoutEnabled: {
        title: this.l('user.lockout-enabled'),
        type: 'boolean',
        ui: {
          grid: {
            span: 6,
          },
        },

      },
      isActive: {
        title: this.l('user.is-active'),
        type: 'boolean',
        ui: {
          grid: {
            span: 6,
          },
        },
      },
    },
    required: [
      'userName',
      'nickname',
      'name',
      'phoneNumber',
      'email'
    ],
    ui: {
      errors: {
        minLength: this.l('validation.minlength'),
        maxLength: this.l('validation.maxlength'),
        required: this.l('validation.required'),
        confirm: this.l('validation.confirm')
      },
      spanLabelFixed: 100,
      grid: {
        span: 12,
      },
    },
  };

  /** 角色表单配置 */
  roleFormSchema: SFSchema = {
    properties: {
      roles: {
        type: 'string',
        title: '',
        ui: {
          widget: 'custom'
        },
      },
    },
    required: [],
    ui: {
      errors: {
        minLength: this.l('validation.minlength'),
        maxLength: this.l('validation.maxlength'),
        required: this.l('validation.required'),
      },
      spanLabelFixed: 1,
      grid: {
        span: 24,
      },
    },
  };

  constructor(
    injector: Injector,
    private userSer: UserServiceProxy,
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
            this.role = {
              roles: res.roles
            };
          }

          if (!this.readonly) {
            this.readonly = res.entityDto.isStatic;
          }
        });
    } else {
      this.pageFormSchema.required.push('password');
      this.pageFormSchema.required.push('passwordConfimd');
    }
  }

  submitForm(...event: any[]) {
    // 构建数据
    const input = new CreateOrEditUserInput({
      entityDto: UserDto.fromJS(event[0]),
      password: event[0].password,
      roles: event[1].roles
    });

    // 发送请求
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
