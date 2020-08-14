import { Component, Injector, OnInit } from '@angular/core';
import { ModalHelper } from '@delon/theme';
import { QueryCondition, QueryInput, RoleDto, RoleServiceProxy, SortCondition, UserDto, UserServiceProxy } from '@service-proxies';
import { ListViewComponentBase } from '@shared/common';
import { finalize } from 'rxjs/operators';
import { CreateOrEditRoleComponent } from './create-or-edit-role';

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

  fetchData(skipCount: number, pageSize: number, queryConditions: QueryCondition[], sortConditions: SortCondition[], callback: (total: number) => void) {
    const queryInput = new QueryInput();
    queryInput.skipCount = skipCount;
    queryInput.pageSize = pageSize;
    queryInput.queryConditions = queryConditions;
    queryInput.sortConditions = sortConditions;

    this.roleSer.getPage(queryInput)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.viewRecord = res.items;
        callback(res.total);
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

}
