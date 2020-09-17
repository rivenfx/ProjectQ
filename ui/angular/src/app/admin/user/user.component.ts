import { Component, Injector, OnInit } from '@angular/core';
import {
  QueryCondition,
  QueryInput,
  SortCondition,
  UserDto,
  UserServiceProxy,
} from '@service-proxies';
import { IFetchPageData, ListViewComponentBase } from '@shared/common';
import * as _ from 'lodash';
import { finalize } from 'rxjs/operators';
import { CreateOrEditUserComponent } from './create-or-edit-user';
import { AppConsts } from '@shared';

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

  fetchData(fetch: IFetchPageData) {
    const queryInput = new QueryInput();
    queryInput.skipCount = fetch.skipCount;
    queryInput.pageSize = fetch.pageSize;

    queryInput.queryConditions = fetch.queryConditions;
    queryInput.sortConditions = fetch.sortConditions;

    this.userSer.getPage(queryInput)
      .pipe(finalize(() => {
        fetch!.finishedCallback();
      }))
      .subscribe((res) => {
        fetch!.successCallback(res);
      });
  }

  create() {
    this.onClickCreateOrEdit();
  }

  edit(data: UserDto) {
    this.onClickCreateOrEdit(data);
  }

  delete(data: UserDto) {
    if (data.isStatic) {
      this.message.warn(this.l('不能删除系统用户'));
      return;
    }

    this.message.confirm(this.l('删除用户 {0}', data.userName), (res) => {
      if (res) {
        this.loading = true;
        this.userSer.delete([data.id])
          .pipe(finalize(() => {
            this.loading = false;
          }))
          .subscribe(() => {
            this.message.success(this.l(AppConsts.message.success));
            this.refresh();
          });
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
