import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutAccountComponent } from '../layout/account';
import { LoginComponent } from './login';
import { RegisterTenantComponent } from './register-tenant';
import { RegisterUserComponent } from './register-user';


const routes: Routes = [
  {
    path: '',
    component: LayoutAccountComponent,
    children: [
      {
        path: 'login',
        component: LoginComponent,
        data: { title: '登录', titleI18n: 'label.login' },
      },
      {
        path: 'register-user',
        component: RegisterUserComponent,
        data: { title: '注册用户', titleI18n: 'label.register-user' },
      },
      {
        path: 'register-tenant',
        component: RegisterTenantComponent,
        data: { title: '注册租户', titleI18n: 'app.register-tenant' },
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'login',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountRoutingModule {
}
