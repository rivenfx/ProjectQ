import { Component, Injector, OnInit } from '@angular/core';
import { ModalHelper } from '@delon/theme';
import {
  QueryCondition,
  QueryInput,
  RoleDto,
  RoleServiceProxy,
  SortCondition,
  UserDto,
  UserServiceProxy,
} from '@service-proxies';
import { IFetchPageData, ListViewComponentBase } from '@shared/common';
import { finalize } from 'rxjs/operators';
import { CreateOrEditRoleComponent } from './create-or-edit-role';
import { AppConsts } from '@shared';
import { STColumn } from '@delon/abc/st';

@Component({
  selector: 'role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.less'],
})
export class RoleComponent extends ListViewComponentBase<RoleDto>
  implements OnInit {

  columns: STColumn[] = [
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
    {
      title: '操作区',
      buttons: [
        {
          tooltip: this.l('common.edit'),
          icon: 'edit',
          type: 'none',
          acl: 'user.edit',
          iif: record => !record.isStatic,
          iifBehavior: 'disabled',
          click: (record) => this.createOrEdit(record),
        },
        {
          tooltip: this.l('common.delete'),
          icon: 'delete',
          type: 'del',
          acl: 'user.delete',
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

  constructor(
    injector: Injector,
    private roleSer: RoleServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    super.ngOnInit();
  }

  fetchData(fetch: IFetchPageData) {
    const queryInput = new QueryInput();
    queryInput.skipCount = fetch.skipCount;
    queryInput.pageSize = fetch.pageSize;

    queryInput.queryConditions = fetch.queryConditions;
    queryInput.sortConditions = fetch.sortConditions;

    this.roleSer.getPage(queryInput)
      .pipe(finalize(() => {
        fetch!.finishedCallback();
      }))
      .subscribe((res) => {
        fetch!.successCallback(res);
      });
  }


  createOrEdit(data?: RoleDto) {
    let input;
    if (data) {
      input = data.id;
    }

    this.modalHelper.createStatic(
      CreateOrEditRoleComponent,
      {
        modalInput: input,
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

}
