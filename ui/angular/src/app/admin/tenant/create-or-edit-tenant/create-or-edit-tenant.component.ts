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
export class CreateOrEditTenantComponent extends ModalComponentBase<string>
  implements OnInit {

  schema: SFSchema = {
    required: [
      'name',
      'displayName',
    ],
    ui: {
      errors: {
        'minLength': this.l('validation.minlength'),
        'maxLength': this.l('validation.maxlength'),
        'required': this.l('validation.required'),
      },
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
      // 'entity.name': {
      //   title: this.l('tenant.display-name'),
      //   type: 'string',
      //   minLength: 4,
      //   maxLength: 32,
      // },
      test: {
        type: 'object',
        properties: {
          name: {
            title: 'test-name',
            type: 'string',
          },
        },
      },
    },
  };
  tmp = {
    name: 'default',
    displayName: 'default',
    test: {
      name: 'test',
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
  }

  submitForm(event?: any) {
    const input = new CreateOrUpdateTenantInput();
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
