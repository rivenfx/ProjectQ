import { Component, Injector, OnInit } from '@angular/core';
import { ModalComponentBase } from '@rivenfx/ng-common';
import { SFSchema } from '@delon/form';
import { CreateTenantInput, TenantServiceProxy } from '@service-proxies';
import { finalize } from 'rxjs/operators';
import { AppConsts } from '@shared';
@Component({
  selector: 'create-tenant',
  templateUrl: './create-tenant.component.html',
  styleUrls: ['./create-tenant.component.less']
})
export class CreateTenantComponent extends ModalComponentBase<string>
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
          'displayName',
          'description'
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
            ui: {
              grid: {
                span: 12,
              },
            }
          },
          displayName: {
            title: this.l('tenant.display-name'),
            type: 'string',
            minLength: 3,
            maxLength: 32,
            ui: {
              grid: {
                span: 12,
              },
            }
          },
          description: {
            title: this.l('label.description'),
            type: 'string',
            minLength: 5,
            maxLength: 512,
            ui: {
              widget: 'textarea',
              autosize: { minRows: 2, maxRows: 6 },
              grid: {
                span: 24,
              },
            }
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


  submitForm(...event: any[]) {
    const input = CreateTenantInput.fromJS(event[0]);
    this.loading = true;
    this.tenantSer.create(input)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(() => {
        this.notify.success(this.l(this.config.message.success));
        this.success();
      });
  }
}
