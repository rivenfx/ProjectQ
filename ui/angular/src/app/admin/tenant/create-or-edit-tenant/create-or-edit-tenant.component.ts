import { Component, Injector, OnInit } from '@angular/core';
import { ModalComponentBase } from '@shared/common';
import { SFSchema } from '@delon/form';
import { CreateOrUpdateTenantInput, TenantServiceProxy } from '@service-proxies';
import { finalize } from 'rxjs/operators';
import { AppConsts } from '@shared';

@Component({
  selector: 'create-or-edit-tenant',
  templateUrl: './create-or-edit-tenant.component.html',
  styleUrls: ['./create-or-edit-tenant.component.less'],
})
export class CreateOrEditTenantComponent extends ModalComponentBase<CreateOrUpdateTenantInput>
  implements OnInit {

  schema: SFSchema = {
    required: [ // 校验字段
      'adminUser',
      'adminUserPassword',
      'adminUserPasswordConfirm',
      'adminUserEmail',
      'adminUserPhoneNumber',
    ],
    ui: { // 全局错误
      errors: {
        'minLength': this.l('validation.minlength'),
        'maxLength': this.l('validation.maxlength'),
        'required': this.l('validation.required'),
      },
    },
    properties: {
      entityDto: { //
        type: 'object',
        required: [ // 校验字段
          'name',
          'displayName',
        ],
        properties: {
          name: {
            title: this.l('tenant.name'),
            type: 'string',
            minLength: 3,
            maxLength: 32,
          },
          displayName: {
            title: this.l('tenant.display-name'),
            type: 'string',
            minLength: 3,
            maxLength: 32,
          },
        },
      },
      adminUser: {
        title: this.l('管理员账号'),
        type: 'string',
        minLength: 3,
        maxLength: 32,
      },
      adminUserPassword: {
        title: this.l('管理员密码'),
        type: 'string',
        minLength: 6,
        maxLength: 32,
      },
      adminUserPasswordConfirm: {
        title: this.l('确认管理员密码'),
        type: 'string',
        minLength: 6,
        maxLength: 32,
        ui: {
          validator: (a, b) => {
            if (b.root && b.root._value && b.root._value['adminUserPassword'] !== a) {
              return [
                { keyword: 'confirm', message: this.l('validation.confirm') },
              ];
            }
            return [];
          },
        },
      },
      adminUserPhoneNumber: {
        title: this.l('管理员电话号码'),
        type: 'string',
        format: 'email',
        minLength: 3,
        maxLength: 32,
      },
      adminUserEmail: {
        title: this.l('管理员邮箱'),
        type: 'string',
        format: 'email',
      },
    },
  };
  tmp = {};

  constructor(
    injector: Injector,
    private tenantSer: TenantServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.title = this.l('tenant');
  }

  formChange(e: any) {
    // debugger
  }

  formValueChange(e: any) {
    // debugger
  }

  submitForm(event?: any) {
    const input = new CreateOrUpdateTenantInput(event);
    debugger
    return;
    this.loading = true;
    this.tenantSer.createOrUpdate(input)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(() => {
        this.notify.success(this.l(AppConsts.message.success));
        this.success();
      });
  }
}
