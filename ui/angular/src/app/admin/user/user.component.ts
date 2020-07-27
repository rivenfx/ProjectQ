import { Component, Injector, OnInit } from '@angular/core';
import { ListViewComponentBase } from '@shared/common';
import { QueryInput, UserDto, UserServiceProxy } from '@service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-user',
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

    this.userSer.getAll(queryInput)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.viewRecord = res.items;
        callback(res.total);
      });
  }

}
