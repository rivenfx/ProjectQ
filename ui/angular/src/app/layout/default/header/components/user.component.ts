import { ChangeDetectionStrategy, Component, Inject, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { SettingsService } from '@delon/theme';
import { AppConsts } from '@shared';
import { IRivenCommonConfig, RIVEN_COMMON_CONFIG, SampleComponentBase } from '@rivenfx/ng-common';

@Component({
  selector: 'header-user',
  template: `
    <div class="alain-default__nav-item d-flex align-items-center px-sm"
         nz-dropdown
         nzPlacement="bottomRight"
         [nzDropdownMenu]="userMenu"
         [title]="settings.user['userName']">
      <nz-avatar [nzSrc]="settings.user.avatar" nzSize="small" class="mr-sm"></nz-avatar>
      {{ settings.user.name }}
    </div>
    <nz-dropdown-menu #userMenu="nzDropdownMenu">
      <div nz-menu class="width-sm">
        <!--        <div nz-menu-item routerLink="/pro/account/center">-->
        <!--          <i nz-icon nzType="user" class="mr-sm"></i>-->
        <!--          {{ l('menu.account.center') }}-->
        <!--        </div>-->
        <!--        <div nz-menu-item routerLink="/pro/account/settings">-->
        <!--          <i nz-icon nzType="setting" class="mr-sm"></i>-->
        <!--          {{ l('menu.account.settings') }}-->
        <!--        </div>-->
        <!--        <div nz-menu-item routerLink="/exception/trigger">-->
        <!--          <i nz-icon nzType="close-circle" class="mr-sm"></i>-->
        <!--          {{ l('menu.account.trigger') }}-->
        <!--        </div>-->
        <!-- <li nz-menu-divider></li> -->
        <div nz-menu-item (click)="logout()">
          <i nz-icon nzType="logout" class="mr-sm"></i>
          {{ l('label.log-out') }}
        </div>
      </div>
    </nz-dropdown-menu>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderUserComponent extends SampleComponentBase {
  constructor(
    injector: Injector,
    public settings: SettingsService,
    public router: Router,
    @Inject(RIVEN_COMMON_CONFIG) public configs: IRivenCommonConfig,
  ) {
    super(injector);
  }

  logout() {
    this.settings.setData(this.configs.settings.token, false);
    this.settings.setData(this.configs.settings.encryptedToken, false);
    this.settings.setData(this.configs.settings.tokenExpiration, false);

    this.router.navigateByUrl(this.configs.routes.loginPage);
  }
}
