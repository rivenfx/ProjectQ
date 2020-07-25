import { AfterViewInit, Component, Inject, Injector, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { AuthenticateModelInput, IAuthenticateModelInput, TokenAuthServiceProxy } from '../../service-proxies';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { Router } from '@angular/router';
import { SessionService } from '@shared/riven';
import { ALAIN_I18N_TOKEN, TitleService } from '@delon/theme';
import { I18nService } from '@core/i18n';
import { AppComponentBase } from '@shared/common';
import { NgForm } from '@angular/forms';
import { ACLService } from '@delon/acl';

@Component({
  selector: 'app-login',
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


  get isEnabledMultiTenancy():boolean{
   return  this.sessionSer.session.multiTenancy.isEnabled;
  }

  constructor(
    injector: Injector,
    @Inject(DA_SERVICE_TOKEN) public tokenService: ITokenService,
    public tokenAuthSer: TokenAuthServiceProxy,
    public router: Router,
    public sessionSer: SessionService,
    private aclService: ACLService,
  ) {
    super(injector);

    this.titleSer.setTitle(this.i18nSer.fanyi('Login'));
  }

  ngOnInit(): void {

  }

  ngAfterViewInit(): void {
    this.tokenService.clear();
  }


  submitForm() {
    this.tokenAuthSer.authenticate(this.input)
      .subscribe((result) => {
        this.tokenService.set({
          token: result.accessToken,
        });

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
