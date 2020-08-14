import { Component, Injector, OnInit } from '@angular/core';
import { PageFilterItemDto, QueryCondition, QueryInput, SortCondition, UserDto, UserServiceProxy } from '@service-proxies';
import { ListViewComponentBase } from '@shared/common';
import { finalize } from 'rxjs/operators';
import { CreateOrEditUserComponent } from './create-or-edit-user';

@Component({
  selector: 'user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.less'],
})
export class UserComponent extends ListViewComponentBase<UserDto>
  implements OnInit {

  constructor(
    injector: Injector,
    private userSer: UserServiceProxy,
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

    this.userSer.getPage(queryInput)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.viewRecord = res.items;
        callback(res.total);
      });
  }

  onClickCreateOrEdit(data?: UserDto) {
    let input;
    if (data) {
      input = data.id;
    }

    this.modalHelper.createStatic(
      CreateOrEditUserComponent,
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
