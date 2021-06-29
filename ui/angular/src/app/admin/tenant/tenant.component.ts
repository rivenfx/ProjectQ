import { Component, Injector, OnInit } from '@angular/core';
import { IFetchPageData, ListViewComponentBase } from '@shared/common';
import {
  QueryInput,
  TenantDto,
  TenantServiceProxy,
} from '@service-proxies';
import { STColumn } from '@delon/abc/st';
import { finalize } from 'rxjs/operators';
import { CreateTenantComponent } from './create-tenant';
import { EditTenantComponent } from './edit-tenant';

@Component({
  selector: 'tenant',
  templateUrl: './tenant.component.html',
  styleUrls: ['./tenant.component.less'],
})
export class TenantComponent extends ListViewComponentBase<TenantDto>
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
      title: this.l('tenant.name'),
      index: 'name',
      sort: true,
    },
    { // 显示名称
      title: this.l('tenant.display-name'),
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
      title: this.l('common.action'),
      buttons: [
        {
          tooltip: this.l('common.view'),
          icon: 'eye',
          type: 'none',
          acl: 'tenant.query',
          click: (record) => this.view(record),
        },
        {
          tooltip: this.l('common.edit'),
          icon: 'edit',
          type: 'none',
          acl: 'tenant.edit',
          iif: record => !record.isStatic,
          iifBehavior: 'disabled',
          click: (record) => this.edit(record),
        },
        {
          tooltip: this.l('common.delete'),
          icon: 'delete',
          type: 'del',
          // acl: 'tenant.delete',
          iif: record => !record.isStatic,
          iifBehavior: 'disabled',
          pop: {
            title: this.l('message.confirm.operation'),
            okType: 'danger',
          },
          click: (record, _modal, comp) => {
            this.message.success(`成功删除【${record.name}】`);
            comp!.removeRow(record);
          },
        },
        // {
        //   text: '更多',
        //   children: [
        //     {
        //       text: record => (record.id === 1 ? `过期` : `正常`),
        //       click: record => this.message.error(`${record.id === 1 ? `过期` : `正常`}【${record.name}】`),
        //     },
        //     {
        //       text: `审核`,
        //       click: record => this.message.info(`check-${record.name}`),
        //       iif: record => record.id % 2 === 0,
        //       iifBehavior: 'disabled',
        //       tooltip: 'This is tooltip',
        //     },
        //     {
        //       type: 'divider',
        //     },
        //     {
        //       text: `重新开始`,
        //       icon: 'edit',
        //       click: record => this.message.success(`重新开始【${record.name}】`),
        //     },
        //   ],
        // },
      ],
    },
  ];

  constructor(
    injector: Injector,
    private tenantServ: TenantServiceProxy,
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

    // displayName 字段筛选条件
    const nameCond = queryInput.queryConditions.find(o => o.field === 'name');
    if (nameCond) {
      const displayNameCond = nameCond.clone();
      displayNameCond.field = 'displayName';
      queryInput.queryConditions.push(displayNameCond);
    }

    this.tenantServ.getPage(queryInput)
      .pipe(finalize(() => {
        fetch.finishedCallback();
      }))
      .subscribe((res) => {
        fetch.successCallback(res);
      });
  }


  create() {

    this.modalHelper.createStatic(
      CreateTenantComponent,
    ).subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }

  edit(data: TenantDto, readonly?: boolean) {
    let input;
    if (data) {
      input = data.id;
    }

    this.modalHelper.createStatic(
      EditTenantComponent,
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

  view(data: TenantDto) {
    this.edit(data, true);
  }
}
