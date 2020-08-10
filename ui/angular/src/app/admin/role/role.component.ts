import { Component, Injector, OnInit } from '@angular/core';
import { ModalHelper } from '@delon/theme';
import { QueryInput, RoleDto, RoleServiceProxy, UserDto, UserServiceProxy } from '@service-proxies';
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

  fetchData(skipCount: number, pageSize: number, callback: (total: number) => void) {
    const queryInput = new QueryInput();
    queryInput.skipCount = skipCount;
    queryInput.pageSize = pageSize;

    this.roleSer.getPage(queryInput)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.viewRecord = res.items;
        callback(res.total);
      });
  }


  onClickEdit(data: RoleDto) {
    this.modalHelper.createStatic(
      CreateOrEditRoleComponent,
      { modalInput: data.id },
    ).subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }

}
