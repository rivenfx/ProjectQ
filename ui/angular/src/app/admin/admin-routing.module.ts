import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from '@shared/riven';
import { LayoutDefaultComponent } from '../layout/default/default.component';
import { DashboardComponent } from './dashboard';
import { TenantComponent } from './tenant';
import { RoleComponent } from './role';
import { UserComponent } from './user';



const routes: Routes = [
  {
    path: '',
    component: LayoutDefaultComponent,
    canActivate: [PermissionGuard],
    canActivateChild: [PermissionGuard],
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'tenant',
        component: TenantComponent,
        data: { permissions: 'tenant.query' },
      },
      {
        path: 'role',
        component: RoleComponent,
        data: { permissions: 'role.query' },
      },
      {
        path: 'user',
        component: UserComponent,
        data: { permissions: 'user.query' },
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
