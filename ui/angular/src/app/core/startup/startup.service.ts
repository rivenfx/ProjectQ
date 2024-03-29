import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Injector } from '@angular/core';
import { ACLService } from '@delon/acl';
import { ALAIN_I18N_TOKEN, MenuService, SettingsService, TitleService } from '@delon/theme';
import { I18nService } from '../i18n';

import { Router } from '@angular/router';
import { SessionDto } from '@service-proxies';
import { AppConsts } from '@shared';
import { SessionService } from '@shared/riven';
import { NzIconService } from 'ng-zorro-antd/icon';
import { ICONS } from '../../../style-icons';
import { ICONS_AUTO } from '../../../style-icons-auto';
import { IRivenCommonConfig, RIVEN_COMMON_CONFIG } from '@rivenfx/ng-common';

/**
 * Used for application startup
 * Generally used to get the basic data of the application, like: Menu Data, User Data, etc.
 */
@Injectable()
export class StartupService {
  constructor(
    iconSrv: NzIconService,
    public menuService: MenuService,
    @Inject(ALAIN_I18N_TOKEN) public i18n: I18nService,
    @Inject(RIVEN_COMMON_CONFIG) public config: IRivenCommonConfig,
    public settingService: SettingsService,
    public aclService: ACLService,
    public titleService: TitleService,
    public httpClient: HttpClient,
    public injector: Injector,
    public router: Router,
  ) {
    iconSrv.addIcon(...ICONS_AUTO, ...ICONS);

    // 自定义setData函数
    function setData(key: string, value: any) {
      if (!this.platform.isBrowser) {
        return;
      }
      localStorage.setItem(key, JSON.stringify(value));
      this.notify$.next({ type: key, name: key, value } as any);
    }

    // 重写 setData 函数实现
    this.settingService.setData = setData;
  }

  load(): Promise<any> {
    // only works with promises
    // https://github.com/angular/angular/issues/15088
    return new Promise((resolve, reject) => {
      this.getAppSettings(resolve, reject);
    });
  }


  /** 加载前端 appsettings.json 配置 */
  private getAppSettings(resolve: any, reject: any) {
    this.httpClient.get('assets/appsettings.json')
      .subscribe(
        (response) => {
          const result = response as any;
          AppConsts.remoteServiceUrl = result.remoteServiceUrl;
          AppConsts.appUrl = result.appUrl;
          this.config.remoteServiceUrl = AppConsts.remoteServiceUrl;
          this.config.appUrl = AppConsts.appUrl;

          this.getAppSession(resolve, reject);
        },
        (e) => {
          reject(e);
        });
  }

  /** 加载会话信息 */
  private getAppSession(resolve: any, reject: any) {
    const sessionSer = this.injector.get(SessionService);

    // 订阅会话数据更改
    sessionSer.sessionChange.subscribe((data) => {
      if (!data) {
        return;
      }

      const token = this.settingService.getData(this.config.settings.token);
      if (token && !data.auth.userId) {
        this.settingService.setData(this.config.settings.token, false);
        this.settingService.setData(this.config.settings.encryptedToken, false);
        this.router.navigateByUrl(this.config.routes.loginPage);
        resolve({});
        return;
      }

      this.initAppInfo(data);

      this.initAclInfo(data);

      this.initUserInfo(data);

      this.initMenuInfo(data);
    });

    // 立即加载会话数据
    sessionSer.loadOrUpdateAppInfo((state, data: SessionDto | any) => {
      if (state) {
        resolve({});
      } else {
        if (reject) {
          reject('init session info error' + data);
        }
      }
    });
  }

  /** 初始化应用信息 */
  private initAppInfo(input: SessionDto) {
    const app: any = {
      name: input.name,
      description: input.name,
    };
    this.settingService.setApp(app);
    this.titleService.suffix = this.settingService.app.name;
  }

  /** 初始化权限信息 */
  private initAclInfo(input: SessionDto) {
    // 权限
    this.aclService.setRole(input.auth.grantedPermissions);
  }

  /** 初始化用户信息 */
  private initUserInfo(input: SessionDto) {
    // 设置登录信息
    const userName = input.auth.userName;
    let displayUserName = input.auth.userNickName;
    if (input.multiTenancy.isEnabled && input.multiTenancy.displayName) {
      displayUserName = input.multiTenancy.displayName + '/' + input.auth.userNickName;
    }
    this.settingService.setUser({
      userName: userName,
      name: displayUserName,
      avatar: 'assets/images/avatar.png',
      // email: 'msmadaoe@msn.com',
    });
  }

  /** 初始化菜单信息 */
  private initMenuInfo(input: SessionDto) {
    const menus = JSON.parse(input.menu);
    this.menuService.add(menus);
  }
}
