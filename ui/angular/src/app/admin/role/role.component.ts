import { Component, Injector, OnInit } from '@angular/core';
import { ModalHelper } from '@delon/theme';
import {
  QueryCondition,
  QueryInput, QueryOperator,
  RoleDto,
  RoleServiceProxy,
  SortCondition,
  UserDto,
  UserServiceProxy,
} from '@service-proxies';
import { IFetchPage, ListComponentBase } from '@rivenfx/ng-common';
import { finalize } from 'rxjs/operators';
import { CreateOrEditRoleComponent } from './create-or-edit-role';
import { AppConsts } from '@shared';
import { STColumn } from '@delon/abc/st';
import { IFetchPage2, ListComponentBase2 } from '@rivenfx/ng-page-filter';
import { ListViewComponentBase } from '@shared/common/list-view-component-base';

@Component({
  selector: 'role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.less'],
})
export class RoleComponent extends ListViewComponentBase<RoleDto>
  implements OnInit {

  constructor(
    injector: Injector,
    private roleSer: RoleServiceProxy,
  ) {
    super(injector);
  }


  initViewConfigs() {
    this.templateFilterSchema.configs = [
      {
        field: 'name',
        operator: QueryOperator.Contains,
        label: 'role.name',
        component: 's-input',
        width: 6,
        args: {
          type: 'text',
        },
      },
      {
        field: 'displayName',
        operator: QueryOperator.Contains,
        label: 'role.display-name',
        component: 's-input',
        width: 6,
        args: {
          type: 'text',
        },
      },
      {
        field: 'description',
        operator: QueryOperator.Contains,
        label: 'role.description',
        component: 's-input',
        width: 6,
        args: {
          type: 'text',
        },
      }
    ];

    this.templateColumns = [
      { // 名称
        title: this.l('role.name'),
        index: 'name',
        sort: true,
      },
      { // 显示名称
        title: this.l('role.display-name'),
        index: 'displayName',
        sort: true,
      },
      { // 是否为内置
        title: this.l('common.is-static'),
        type: 'badge',
        index: 'isStatic',
        badge: {
          true: { text: this.l('label.yes'), color: 'success' },
          false: { text: this.l('label.no'), color: 'error' },
        },
        sort: true,
      },
    ];
  }

  processColumns(columns: STColumn<any>[]): STColumn<any>[] {
    return [
      // 前置列
      { // no 列
        index: '',
        title: 'No',
        type: 'no',
        fixed: 'left',
        width: 40,
      },
      { // checkbox 列
        title: '',
        index: '',
        type: 'checkbox',
        fixed: 'left',
        width: 30,
      },

      // 中间列
      ...columns,

      // 结束列
      {
        title: this.l('common.action'),
        buttons: [
          {
            tooltip: this.l('common.view'),
            icon: 'eye',
            type: 'none',
            acl: 'role.query',
            click: (record) => this.view(record),
          },
          {
            tooltip: this.l('common.edit'),
            icon: 'edit',
            type: 'none',
            acl: 'role.edit',
            iif: record => !record.isStatic,
            iifBehavior: 'disabled',
            click: (record) => this.createOrEdit(record),
          },
          {
            tooltip: this.l('common.delete'),
            icon: 'delete',
            type: 'del',
            acl: 'role.delete',
            iif: record => !record.isStatic,
            iifBehavior: 'disabled',
            pop: {
              title: this.l('message.confirm.operation'),
              okType: 'danger',
            },
            click: (record, _modal, comp) => {
              this.delete(record);
            },
          },
        ],
      },
    ];
  }


  ngOnInit(): void {
    super.ngOnInit();
  }



  view(data: RoleDto) {
    this.createOrEdit(data, true);
  }

  createOrEdit(data?: RoleDto, readonly?: boolean) {
    let input;
    if (data) {
      input = data.id;
    }

    this.modalHelper.createStatic(
      CreateOrEditRoleComponent,
      {
        modalInput: input,
        readonly: readonly,
      },
    ).subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }

  delete(data?: RoleDto) {
    if (data.isStatic) {
      this.message.warn(this.l('不能删除系统角色'));
      return;
    }

    this.message.confirm(this.l('删除角色 {0}', data.name), (res) => {
      if (res) {
        this.loading = true;
        this.roleSer.delete([data.id])
          .pipe(finalize(() => {
            this.loading = false;
          }))
          .subscribe(() => {
            this.notify.success(this.l(AppConsts.message.success));
            this.refresh();
          });
      }
    });
  }

  fetchData(fetch: IFetchPage2): void {
    const queryInput = new QueryInput();
    queryInput.skipCount = fetch.skipCount;
    queryInput.pageSize = fetch.pageSize;

    queryInput.queryConditions = fetch.queryConditions.map(o => {
      return QueryCondition.fromJS(o);
    });
    queryInput.sortConditions = fetch.sortConditions.map(o => {
      return SortCondition.fromJS(o);
    });

    this.roleSer.getPage(queryInput)
      .pipe(finalize(() => {
        fetch!.finishedCallback();
      }))
      .subscribe((res) => {
        fetch!.successCallback(res);
      });
  }

}
