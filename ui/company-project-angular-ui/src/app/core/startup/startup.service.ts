import { Injectable, Injector, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MenuService, SettingsService, TitleService, ALAIN_I18N_TOKEN } from '@delon/theme';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { ACLService } from '@delon/acl';
import { I18nService } from '../i18n';

import { NzIconService } from 'ng-zorro-antd/icon';
import { ICONS } from '../../../style-icons';
import { ICONS_AUTO } from '../../../style-icons-auto';
import { SessionService } from '../../shared/riven';
import { AppConsts } from '@shared';
import { SessionDto } from '../../service-proxies';

/**
 * Used for application startup
 * Generally used to get the basic data of the application, like: Menu Data, User Data, etc.
 */
@Injectable()
export class StartupService {
  constructor(
    iconSrv: NzIconService,
    private menuService: MenuService,
    @Inject(ALAIN_I18N_TOKEN) private i18n: I18nService,
    private settingService: SettingsService,
    private aclService: ACLService,
    private titleService: TitleService,
    @Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
    private httpClient: HttpClient,
    private injector: Injector,
  ) {
    iconSrv.addIcon(...ICONS_AUTO, ...ICONS);
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
          var result = response as any;
          AppConsts.remoteServiceUrl = result.remoteServiceUrl;
          AppConsts.appUrl = result.appUrl;

          this.getAppSession(resolve, reject);
        },
        (e) => {
          reject(e);
        });
  }

  /** 加载会话信息 */
  private getAppSession(resolve: any, reject: any) {
    const sessionSer = this.injector.get(SessionService);
    sessionSer.loadOrUpdateAppInfo((state, data: SessionDto | any) => {
      if (state) {
        if (resolve) {
          this.viaMock(resolve, reject);
          // resolve(null);
        }
      } else {
        if (reject) {
          reject('init session info error' + data);
        }
      }
    });
  }


  private viaMock(resolve: any, reject: any) {
    // const tokenData = this.tokenService.get();
    // if (!tokenData.token) {
    //   this.injector.get(Router).navigateByUrl('/passport/login');
    //   resolve({});
    //   return;
    // }
    // mock
    const app: any = {
      name: `ng-alain`,
      description: `Ng-zorro admin panel front-end framework`,
    };
    const user: any = {
      name: 'Admin',
      avatar: './assets/tmp/img/avatar.jpg',
      email: 'cipchk@qq.com',
      token: '123456789',
    };
    // Application information: including site name, description, year
    this.settingService.setApp(app);
    // User information: including name, avatar, email address
    this.settingService.setUser(user);
    // ACL: Set the permissions to full, https://ng-alain.com/acl/getting-started
    this.aclService.setFull(true);
    // Menu data, https://ng-alain.com/theme/menu
    this.menuService.add([
      {
        text: 'Main',
        group: true,
        children: [
          {
            text: 'Dashboard',
            link: '/dashboard',
            icon: { type: 'icon', value: 'appstore' },
          },
          {
            text: 'Quick Menu',
            icon: { type: 'icon', value: 'rocket' },
            shortcutRoot: true,
          },
        ],
      },
    ]);
    // Can be set page suffix title, https://ng-alain.com/theme/title
    this.titleService.suffix = app.name;

    resolve({});
  }


}
