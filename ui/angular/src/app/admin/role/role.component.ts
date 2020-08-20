import { Component, Injector, OnInit } from '@angular/core';
import { ModalHelper } from '@delon/theme';
import { QueryCondition, QueryInput, RoleDto, RoleServiceProxy, SortCondition, UserDto, UserServiceProxy } from '@service-proxies';
import { IFetchData, ListViewComponentBase } from '@shared/common';
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

  }

  fetchData(arg: IFetchData) {
    const queryInput = new QueryInput();
    queryInput.skipCount = arg.skipCount;
    queryInput.pageSize = arg.pageSize;

    queryInput.queryConditions = arg.queryConditions;
    queryInput.sortConditions = arg.sortConditions;

    this.roleSer.getPage(queryInput)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.viewRecord = res.items;
        arg.callback(res.total);
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
