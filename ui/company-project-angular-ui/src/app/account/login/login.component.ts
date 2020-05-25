import { AfterViewInit, Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { SFSchema } from '@delon/form';
import { AuthenticateModelInput, IAuthenticateModelInput, TokenAuthServiceProxy } from '../../service-proxies';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls:['./login.component.less']
})
export class LoginComponent implements OnInit, AfterViewInit {

  input = new AuthenticateModelInput();

  constructor(
    public tokenAuthSer: TokenAuthServiceProxy,
  ) {
    this.input.useToken = true;
  }

  ngOnInit(): void {

  }

  ngAfterViewInit(): void {

  }


  submitForm() {
    debugger
    this.tokenAuthSer.authenticate(this.input)
      .subscribe((result) => {

      });
  }


}
