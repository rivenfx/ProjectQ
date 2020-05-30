import { AfterViewInit, Component, Inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { SFSchema } from '@delon/form';
import { AuthenticateModelInput, IAuthenticateModelInput, TokenAuthServiceProxy } from '../../service-proxies';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { Router } from '@angular/router';
import { SessionService } from '../../shared/riven';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
})
export class LoginComponent implements OnInit, AfterViewInit {

  input = new AuthenticateModelInput();

  constructor(
    @Inject(DA_SERVICE_TOKEN) public tokenService: ITokenService,
    public tokenAuthSer: TokenAuthServiceProxy,
    public router: Router,
    public sessionSer: SessionService,
  ) {
    this.input.useToken = true;
    this.input.useCookie = false;
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
            this.router.navigateByUrl('/admin/dashboard')
              .then(r => {

              });
          }
        });

      });
  }


}
