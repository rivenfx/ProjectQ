import { Component, Injector, OnInit } from '@angular/core';
import {
  QueryCondition,
  QueryInput,
  SortCondition,
  UserDto,
  UserServiceProxy,
} from '@service-proxies';
import { IFetchPageData, ListViewComponentBase } from '@shared/common';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';
import { CreateOrEditUserComponent } from './create-or-edit-user';
import { AppConsts } from '@shared';
import { STColumn } from '@delon/abc/st';

@Component({
  selector: 'user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.less'],
})
export class UserComponent extends ListViewComponentBase<UserDto>
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
      title: this.l('user.user-name'),
      index: 'userName',
      sort: true,
    },
    { // 昵称
      title: this.l('user.nick-name'),
      index: 'nickname',
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
    { // 是否激活
      title: this.l('common.is-active'),
      type: 'badge',
      index: 'isActive',
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
    private userSer: UserServiceProxy,
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

    this.userSer.getPage(queryInput)
      .pipe(finalize(() => {
        fetch!.finishedCallback();
      }))
      .subscribe((res) => {
        fetch!.successCallback(res);
      });
  }

  delete(data: UserDto) {
    if (data.isStatic) {
      this.message.warn(this.l('不能删除系统用户'));
      return;
    }

    this.message.confirm(this.l('删除用户 {0}', data.userName), (res) => {
      if (res) {
        this.loading = true;
        this.userSer.delete([data.id])
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

  view(data: UserDto) {
    this.createOrEdit(data, true);
  }

  createOrEdit(data?: UserDto, readonly?: boolean) {
    let input;
    if (data) {
      input = data.id;
    }

    this.modalHelper.createStatic(
      CreateOrEditUserComponent,
      {
        modalInput: input,
        // tslint:disable-next-line: object-literal-shorthand
        readonly: readonly,
      },
    ).subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }

}
