import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClaimsGuard } from '@shared/riven';
import { LayoutDefaultComponent } from '../layout/default/default.component';
import { DashboardComponent } from './dashboard';
import { RoleComponent } from './role';
import { UserComponent } from './user';


const routes: Routes = [
  {
    path: '',
    component: LayoutDefaultComponent,
    canActivate: [ClaimsGuard],
    canActivateChild: [ClaimsGuard],
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'role',
        component: RoleComponent,
        data: { claims: 'role.query' },
      },
      {
        path: 'user',
        component: UserComponent,
        data: { claims: 'user.query' },
      },
      {
        path: '**',
        redirectTo: 'dashboard',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {
}
