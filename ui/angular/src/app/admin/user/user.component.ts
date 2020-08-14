import { Component, Injector, OnInit } from '@angular/core';
import { PageFilterItemDto, QueryCondition, QueryInput, UserDto, UserServiceProxy } from '@service-proxies';
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

  fetchData(skipCount: number, pageSize: number, callback: (total: number) => void) {
    const queryInput = new QueryInput();
    queryInput.skipCount = skipCount;
    queryInput.pageSize = pageSize;

    queryInput.queryConditions = this.queryConditions;
    queryInput.sortConditions = this.sortConditions;

    this.userSer.getPage(queryInput)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.viewRecord = res.items;
        callback(res.total);
      });
  }

  onClickAdd() {
    this.modalHelper.createStatic(
      CreateOrEditUserComponent).subscribe((res) => {
        if (res) {
          this.refresh();
        }
      });
  }

  onClickEdit(data: UserDto) {
    this.modalHelper.createStatic(
      CreateOrEditUserComponent,
      {
        modalInput: data.id,
      },
    ).subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }

}
