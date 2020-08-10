import { Component, Injector, OnInit } from '@angular/core';
import { ModalHelper, SettingsService } from '@delon/theme';
import { TenantServiceProxy } from '@service-proxies';
import { AppComponentBase } from '@shared/common';
import { RequestHelper } from '@shared/riven/helper';
import { TenantChangeModalComponent } from './tenant-change-modal';

@Component({
  selector: 'tenant-change',
  templateUrl: './tenant-change.component.html',
  styleUrls: ['./tenant-change.component.less'],
})
export class TenantChangeComponent extends AppComponentBase
  implements OnInit {

  tenantName: string;

  constructor(
    injector: Injector,
    public settingsSer: SettingsService,
    public modalHelper: ModalHelper,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.tenantName = this.settingsSer.getData(RequestHelper.multiTenancy.key);
  }

  openChangeModal() {
    this.modalHelper.createStatic(TenantChangeModalComponent,
      {},
      {
        size: 'sm',
      })
      .subscribe((res) => {
        this.tenantName = this.settingsSer.getData(RequestHelper.multiTenancy.key);
      });
  }
}
