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

  pageFormSchema: SFSchema = {
    required: [ // 校验字段
      'adminUser',
      'adminUserPassword',
      'adminUserPasswordConfirm',
      'adminUserEmail',
      'adminUserPhoneNumber',
    ],
    ui: { // 全局错误
      errors: {
        minLength: this.l('validation.minlength'),
        maxLength: this.l('validation.maxlength'),
        required: this.l('validation.required'),
      },
      spanLabelFixed: 100,
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
        ui: {

        }
      },
      adminUser: {
        title: this.l('user.user-name'),
        type: 'string',
        minLength: 3,
        maxLength: 32,
      },
      adminUserPassword: {
        title: this.l('label.password'),
        type: 'string',
        minLength: 6,
        maxLength: 32,
        ui: {
          type: 'password'
        }
      },
      adminUserPasswordConfirm: {
        title: this.l('label.password-confirm'),
        type: 'string',
        minLength: 6,
        maxLength: 32,
        ui: {
          ui: {
            type: 'password'
          },
          validator: (val, fp, f) => {
            if (val !== f.getProperty('adminUserPassword').value) {
              return [
                { keyword: 'confirm', message: this.l('validation.confirm') },
              ];
            }
            return [];
          },
        },
      },
      adminUserPhoneNumber: {
        title: this.l('user.phone-number'),
        type: 'string',
        // format: 'mobile',
        minLength: 3,
        maxLength: 32,
      },
      adminUserEmail: {
        title: this.l('user.email'),
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

  submitForm(...event: any[]) {
    const input = CreateOrUpdateTenantInput.fromJS(event[0]);
    debugger;
    return;
    // this.loading = true;
    // this.tenantSer.createOrUpdate(input)
    //   .pipe(finalize(() => {
    //     this.loading = false;
    //   }))
    //   .subscribe(() => {
    //     this.notify.success(this.l(AppConsts.message.success));
    //     this.success();
    //   });
  }
}
