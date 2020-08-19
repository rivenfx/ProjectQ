import { Component, Injector, OnInit } from '@angular/core';
import {
  ListViewServiceProxy,
  PageFilterItemDto,
  QueryCondition,
  QueryInput,
  SortCondition,
  UserDto,
  UserServiceProxy,
} from '@service-proxies';
import { ListViewComponentBase } from '@shared/common';
import { ISampleTableAction } from '@shared/components/sample-components/sample-table';
import * as _ from 'lodash';
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

  onPageNameChange(name: string) {
    this.fetchPageFilter(name, () => {
      this.fetchListView(name, () => {
        this.refresh();
      });
    });
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

  create() {
    this.onClickCreateOrEdit();
  }

  edit(data: UserDto) {
    this.onClickCreateOrEdit(data);
  }

  delete(data: UserDto) {
    this.message.confirm(this.l('是否删除'), (res) => {
      if (res) {
        this.loading = true;
        this.userSer.delete([data.id])
          .pipe(finalize(() => {
            this.loading = false;
          }))
          .subscribe(() => {
            this.message.success(this.l(this.appConsts.message.success));
          });
      } else {
        this.message.success(this.l(this.appConsts.message.cancelled));
      }
    });
  }

  view(data: UserDto) {
    this.onClickCreateOrEdit(data, true);
  }

  onClickCreateOrEdit(data?: UserDto, readonly?: boolean) {
    let input;
    if (data) {
      input = data.id;
    }

    this.modalHelper.createStatic(
      CreateOrEditUserComponent,
      {
        modalInput: input,
        // tslint:disable-next-line: object-literal-shorthand
        readonly: readonly
      },
    ).subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }

}
