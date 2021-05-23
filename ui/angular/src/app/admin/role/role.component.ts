import { Component, Injector, OnInit } from '@angular/core';
import { ModalHelper } from '@delon/theme';
import { QueryCondition, QueryInput, RoleDto, RoleServiceProxy, SortCondition, UserDto, UserServiceProxy } from '@service-proxies';
import { IFetchPageData, ListViewComponentBase } from '@shared/common';
import { finalize } from 'rxjs/operators';
import { CreateOrEditRoleComponent } from './create-or-edit-role';
import { AppConsts } from '@shared';

@Component({
  selector: 'role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.less'],
})
export class RoleComponent extends ListViewComponentBase<RoleDto>
  implements OnInit {

  constructor(
    injector: Injector,
    private roleSer: RoleServiceProxy
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


  onClickCreateOrEdit(data?: RoleDto) {
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

  onDelete(data?: RoleDto) {
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
