import { Component, Injector, OnInit } from '@angular/core';
import { IFetchPageData, ListViewComponentBase } from '@shared/common';
import {
  QueryCondition,
  QueryInput,
  SortCondition,
  SortType,
  TenantDto,
  TenantServiceProxy,
  UserDto,
} from '@service-proxies';
import { STChange, STColumn, STData } from '@delon/abc/st';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'tenant',
  templateUrl: './tenant.component.html',
  styleUrls: ['./tenant.component.less'],
})
export class TenantComponent extends ListViewComponentBase<TenantDto>
  implements OnInit {

  columns: STColumn[] = [
    {
      title: 'No',
      index: 'id',
      type: 'checkbox',
    },
    { // 名称
      title: this.l('tenant.name'),
      index: 'name',
    },
    { // 显示名称
      title: this.l('tenant.display-name'),
      index: 'displayName',
    },
    { // 是否为内置
      title: this.l('common.is-static'),
      type: 'badge',
      index: 'isStatic',
      badge: {
        'true': { text: this.l('label.yes'), color: 'success' },
        'false': { text: this.l('label.no'), color: 'error' },
      },
    },
    { // 是否激活
      title: this.l('common.is-active'),
      type: 'badge',
      index: 'isActive',
      badge: {
        'true': { text: this.l('label.yes'), color: 'success' },
        'false': { text: this.l('label.no'), color: 'error' },
      },
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

  /** 创建或编辑 */
  createOrEdit(entity?: TenantDto) {
  }
}
