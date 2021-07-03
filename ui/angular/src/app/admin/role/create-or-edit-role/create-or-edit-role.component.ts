import { Component, Injector, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';
import { CreateOrUpdateRoleInput, RoleDto, RoleServiceProxy } from '@service-proxies';
import { ModalComponentBase } from '@rivenfx/ng-common';
import { finalize } from 'rxjs/operators';
import { AppConsts } from '@shared';

@Component({
  selector: 'create-or-edit-role',
  templateUrl: './create-or-edit-role.component.html',
  styleUrls: ['./create-or-edit-role.component.less'],
})
export class CreateOrEditRoleComponent extends ModalComponentBase<string>
  implements OnInit {

  role = new RoleDto();

  permission = {
    permissions: []
  };



  /** 基本表单配置 */
  pageFormSchema: SFSchema = {
    properties: {
      name: {
        type: 'string',
        title: this.l('role.name'),
        minLength: 5,
      },
      displayName: {
        title: this.l('role.display-name'),
        type: 'string',
        minLength: 5,
      },
      description: {
        title: this.l('role.description'),
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
    },
    required: [
      'name',
      'displayName',
      'description'
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


  /** 权限表单配置 */
  permissionFormSchema: SFSchema = {
    properties: {
      permissions: {
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
    public roleSer: RoleServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.title = this.l('role');
    if (this.modalInput) {
      this.loading = true;
      this.roleSer.getEditById(this.modalInput)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe((res) => {
          this.role = res.entityDto;
          this.permission = {
            permissions: res.permissions
          };

          if (!this.readonly) {
            this.readonly = this.role.isStatic;
          }
          this.pageFormSchema.properties.name.readOnly = true;
        });
    }
  }

  submitForm(...event: any[]) {
    // 构建数据
    const input = new CreateOrUpdateRoleInput({
      entityDto: RoleDto.fromJS(event[0]),
      permissions: event[1].permissions
    });

    this.loading = true;
    if (this.modalInput) {
      this.roleSer.update(input)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(() => {
          this.notify.success(this.l(this.config.message.success));
          this.success();
        });
    } else {
      this.roleSer.create(input)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(() => {
          this.notify.success(this.l(this.config.message.success));
          this.success();
        });
    }

  }
}
