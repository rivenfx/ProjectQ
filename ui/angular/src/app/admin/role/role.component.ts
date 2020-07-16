import { Component, Injector, OnInit } from '@angular/core';
import { ListViewComponentBase } from '@shared/common';
import { QueryInput, RoleDto, RoleServiceProxy, UserDto, UserServiceProxy } from '@service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-role',
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

  ngOnInit(): void {
    super.ngOnInit();
  }

  fetchData(skipCount: number, pageSize: number, callback: (total: number) => void) {
    const queryInput = new QueryInput();
    queryInput.skipCount = skipCount;
    queryInput.pageSize = pageSize;

    this.roleSer.getAll(queryInput)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.viewData = res.items;
        callback(res.total);
      });
  }

}
