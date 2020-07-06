import { ChangeDetectionStrategy, Component, HostListener, Injector } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalService } from 'ng-zorro-antd/modal';
import { SampleComponentBase } from '@shared';

@Component({
  selector: 'header-storage',
  template: `
    <i nz-icon nzType="tool"></i>
    {{ l('menu.clear.local.storage') }}
  `,
  // tslint:disable-next-line: no-host-metadata-property
  host: {
    '[class.d-block]': 'true',
  },
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderStorageComponent extends SampleComponentBase {
  constructor(
    injector: Injector,
    private modalSrv: NzModalService,
    private messageSrv: NzMessageService,
  ) {
    super(injector);
  }

  @HostListener('click')
  _click() {
    this.modalSrv.confirm({
      nzTitle: 'Make sure clear all local storage?',
      nzOnOk: () => {
        localStorage.clear();
        this.messageSrv.success('Clear Finished!');
      },
    });
  }
}
