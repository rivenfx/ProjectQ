import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login';
import { LayoutAccountComponent } from '../layout/account';


const routes: Routes = [
  {
    path: '',
    component: LayoutAccountComponent,
    children: [
      {
        path: 'login',
        component: LoginComponent,
        data: { title: '登录', titleI18n: 'app.login.login' }
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
