import { Component, Injector, OnInit } from '@angular/core';
import { ListViewComponentBase } from '@shared/common';
import { PageFilterItemDto, QueryInput, UserDto, UserServiceProxy } from '@service-proxies';
import { finalize } from 'rxjs/operators';
import { CreateOrEditUserComponent } from './create-or-edit-user';

@Component({
  selector: 'user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.less'],
})
export class UserComponent extends ListViewComponentBase<UserDto>
  implements OnInit {

  pageFilters: PageFilterItemDto[] = [
    new PageFilterItemDto({
      type: 'basic-input',
      name: 'userName',
      label: 'user.user-name',
      required: false,
      args: {
        type: 'text',
      },
      valueChange: [],
      order: 0,
      advanced: false,
      enabled: true,
      width: 8,
      xsWidth: undefined,
      smWidth: undefined,
      mdWidth: undefined,
      lgWidth: undefined,
      xlWidth: undefined,
      xxlWidth: undefined,
    }),
  ];

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

    this.userSer.getPage(queryInput)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.viewRecord = res.items;
        callback(res.total);
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
