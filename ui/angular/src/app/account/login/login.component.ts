import { AfterViewInit, Component, Inject, Injector, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SettingsService } from '@delon/theme';
import { AuthenticateModelInput, IAuthenticateModelInput, TokenAuthServiceProxy } from '@service-proxies';
import { AppConsts } from '@shared';
import { AppComponentBase } from '@shared/common';
import { SessionService } from '@shared/riven';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
})
export class LoginComponent extends AppComponentBase
  implements OnInit, AfterViewInit {

  input = new AuthenticateModelInput({
    account: undefined,
    password: undefined,
    verificationCode: undefined,
    returnUrl: undefined,
    rememberClient: false,
    useToken: true,
    useCookie: false,
  });


  get isEnabledMultiTenancy(): boolean {
    return this.sessionSer.session.multiTenancy.isEnabled;
  }

  constructor(
    injector: Injector,
    public tokenAuthSer: TokenAuthServiceProxy,
    public router: Router,
    public sessionSer: SessionService,
    private settingSer: SettingsService,
  ) {
    super(injector);

    this.titleSer.setTitle(this.i18nSer.fanyi('Login'));
  }

  ngOnInit(): void {
    // 重置token过期时间
    this.settingSer.setData(AppConsts.settings.tokenExpiration, false);
  }

  ngAfterViewInit(): void {
    // 重置token
    this.settingSer.setData(AppConsts.settings.token, false);
    this.settingSer.setData(AppConsts.settings.encryptedToken, false);
  }


  submitForm() {

    this.loading = true;

    this.tokenAuthSer.authenticate(this.input)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((result) => {
        // 更新token
        this.settingSer.setData(AppConsts.settings.token, result.accessToken);
        this.settingSer.setData(AppConsts.settings.encryptedToken, result.encryptedAccessToken);
        // 更新token过期时间
        const date = new Date();
        date.setSeconds(date.getSeconds() + result.expireInSeconds);
        this.settingSer.setData(AppConsts.settings.tokenExpiration, date.valueOf());

        this.sessionSer.loadOrUpdateAppInfo((state, data) => {
          if (state) {
            if (result.returnUrl) {
              window.location.href = result.returnUrl;
            } else {
              this.router.navigateByUrl(this.appConsts.urls.mainPage)
                .then(r => {

                });
            }
          }
        });

      });
  }


}
