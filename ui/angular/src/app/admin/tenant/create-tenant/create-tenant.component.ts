import { Component, Injector, OnInit } from '@angular/core';
import { ModalComponentBase } from '@shared/common';
import { SFSchema } from '@delon/form';
import { CreateOrUpdateTenantInput, TenantServiceProxy } from '@service-proxies';
import { finalize } from 'rxjs/operators';
import { AppConsts } from '@shared';
@Component({
  selector: 'create-tenant',
  templateUrl: './create-tenant.component.html',
  styleUrls: ['./create-tenant.component.less']
})
export class CreateTenantComponent extends ModalComponentBase<CreateOrUpdateTenantInput>
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
      spanLabelFixed: 120,
    },
    properties: {
      entityDto: { //
        type: 'object',
        required: [ // 校验字段
          'name',
          'displayName'
        ],
        if: {
          properties: {
            useConnectionString: { enum: [false] }
          }
        },
        then: {
          required: [

          ]
        },
        else: {
          required: [
            'connectionString'
          ]
        },
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
          useConnectionString: {
            title: this.l('指定数据库'),
            type: 'boolean',
            enum: [
              { label: this.l('label.no'), value: false },
              { label: this.l('label.yes'), value: true }
            ],
            ui: {
              widget: 'radio',
              styleType: 'button'
            },
            default: false
          },
          connectionString: {
            title: this.l('数据库连接字符串'),
            type: 'string',
            minLength: 3,
            maxLength: 32,
          },
          isActive: {
            title: this.l('common.is-active'),
            type: 'boolean',
            ui: {
              grid: {
                span: 6,
              },
            },
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
          type: 'password',
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
