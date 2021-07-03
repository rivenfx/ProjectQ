import { ChangeDetectionStrategy, Component, Inject, Injector } from '@angular/core';
import { SettingsService } from '@delon/theme';
import { IRivenCommonConfig, RIVEN_COMMON_CONFIG, SampleComponentBase } from '@rivenfx/ng-common';
import { Router } from '@angular/router';
import { AppConsts } from '@shared';

@Component({
  selector: 'layout-sidebar',
  templateUrl: './sidebar.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent extends SampleComponentBase {
  constructor(
    injector: Injector,
    public router: Router,
    public settings: SettingsService,
    @Inject(RIVEN_COMMON_CONFIG) public config: IRivenCommonConfig,
  ) {
    super(injector);
  }

  logout() {
    this.settings.setData(this.config.settings.token, false);
    this.settings.setData(this.config.settings.encryptedToken, false);
    this.settings.setData(this.config.settings.tokenExpiration, false);
    this.router.navigateByUrl(this.config.routes.loginPage);
  }
}
