import { ChangeDetectionStrategy, Component, Injector } from '@angular/core';
import { SettingsService } from '@delon/theme';
import { SampleComponentBase } from '@rivenfx/ng-common';
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
    private router: Router,
    public settings: SettingsService,
  ) {
    super(injector);
  }

  logout() {
    this.settings.setData(AppConsts.settings.token, false);
    this.settings.setData(AppConsts.settings.encryptedToken, false);
    this.settings.setData(AppConsts.settings.tokenExpiration, false);
    this.router.navigateByUrl(AppConsts.urls.loginPage);
  }
}
