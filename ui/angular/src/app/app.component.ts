import { Component, ElementRef, Inject, OnInit, Renderer2 } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { SettingsService, TitleService, VERSION as VERSION_ALAIN } from '@delon/theme';
import { TokenAuthServiceProxy } from '@service-proxies';
import { AppConsts } from '@shared';
import { NzModalService } from 'ng-zorro-antd/modal';
import { VERSION as VERSION_ZORRO } from 'ng-zorro-antd/version';
import { filter } from 'rxjs/operators';
import { ReuseTabService } from '@delon/abc/reuse-tab';
import { IRivenCommonConfig, RIVEN_COMMON_CONFIG } from '@rivenfx/ng-common';

@Component({
  selector: 'app-root',
  template: ` <router-outlet></router-outlet> `,
})
export class AppComponent implements OnInit {
  refreshTokenTimer: any;

  constructor(
    el: ElementRef,
    renderer: Renderer2,
    @Inject(RIVEN_COMMON_CONFIG) public config: IRivenCommonConfig,
    private router: Router,
    private titleSrv: TitleService,
    private reuseTabSrv: ReuseTabService,
    private modalSrv: NzModalService,
    private settingSer: SettingsService,
    private tokenAuthSer: TokenAuthServiceProxy,
  ) {
    renderer.setAttribute(el.nativeElement, 'ng-alain-version', VERSION_ALAIN.full);
    renderer.setAttribute(el.nativeElement, 'ng-zorro-version', VERSION_ZORRO.full);
  }

  ngOnInit() {
    this.router.events.pipe(filter((evt) => evt instanceof NavigationEnd)).subscribe((e) => {
      const event = e as NavigationEnd;
      if (event.url === this.config.routes.loginPage) {
        this.reuseTabSrv.clear(true);
      }
      this.titleSrv.setTitle();
      this.modalSrv.closeAll();
    });

    this.registerRefreshToken();
  }

  /** 注册刷新token的函数 */
  registerRefreshToken() {
    // 等待刷新token
    this.waitRefreshToken();

    // 注册事件,订阅token过期刷新
    this.settingSer.notify.subscribe((setting) => {
      if (setting.name === this.config.settings.tokenExpiration) {
        clearTimeout(this.refreshTokenTimer);
        this.refreshTokenTimer = null;
        this.waitRefreshToken();
      }
    });
  }

  /** 创建定时器, */
  private waitRefreshToken() {
    if (this.refreshTokenTimer) {
      return;
    }

    const tokenExpirationTimeStamp = this.settingSer.getData(this.config.settings.tokenExpiration);
    if (typeof tokenExpirationTimeStamp !== 'number') {
      try {
        clearTimeout(this.refreshTokenTimer);
        this.refreshTokenTimer = null;
      } catch (e) {}
      return;
    }

    const currentTimeStamp = Date.now();
    if (currentTimeStamp > tokenExpirationTimeStamp) {
      return;
    }

    // 计时刷新
    const refreshMS = tokenExpirationTimeStamp - currentTimeStamp;
    this.refreshTokenTimer = setTimeout(() => {
      clearTimeout(this.refreshTokenTimer);
      this.refreshTokenTimer = null;

      this.tokenAuthSer.refreshToken().subscribe((result) => {
        // 更新token
        this.settingSer.setData(this.config.settings.token, result.accessToken);
        this.settingSer.setData(this.config.settings.encryptedToken, result.encryptedAccessToken);
        // 更新token过期时间
        const date = new Date();
        date.setSeconds(date.getSeconds() + result.expireInSeconds);
        this.settingSer.setData(this.config.settings.tokenExpiration, date.valueOf());
      });
    }, refreshMS);
  }
}
