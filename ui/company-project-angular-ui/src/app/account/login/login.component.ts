import { Component, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: [],
})
export class LoginComponent implements OnInit {

  schema: SFSchema = {
    properties: {
      email: {
        type: 'string',
        title: '账号',
        format: 'email',
        maxLength: 32,
        minLength: 3,
      },
      name: {
        type: 'string',
        title: '密码',
        minLength: 6,
      },
    },
  };

  constructor() {
    debugger
  }

  ngOnInit(): void {
  }


  submit(value: any) {

  }

}
