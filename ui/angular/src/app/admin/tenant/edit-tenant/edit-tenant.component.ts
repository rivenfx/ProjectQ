import { Component, Injector, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';
import { TenantEditDto, TenantDto, TenantServiceProxy } from '@service-proxies';
import { AppConsts } from '@shared';
import { ModalComponentBase } from '@rivenfx/ng-common';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'edit-tenant',
  templateUrl: './edit-tenant.component.html',
  styleUrls: ['./edit-tenant.component.less']
})
export class EditTenantComponent extends ModalComponentBase<string>
  implements OnInit {

  tenant = new TenantEditDto({
    entityDto: new TenantDto()
  });


  pageFormSchema: SFSchema = {
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
      }
    },
  };

  constructor(
    injector: Injector,
    private tenantSer: TenantServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.title = this.l('tenant');

    if (this.modalInput) {
      this.loading = true;
      this.tenantSer.getEditById(this.modalInput)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe((res) => {
          this.tenant = res;
        });
    }
  }


  submitForm(...event: any[]) {
    const input = TenantEditDto.fromJS(event[0]);
    this.loading = true;
    this.tenantSer.update(input)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(() => {
        this.notify.success(this.l(this.config.message.success));
        this.success();
      });
  }

}
