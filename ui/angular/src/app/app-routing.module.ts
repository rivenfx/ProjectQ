import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'account',
    loadChildren: () => import('./account/account.module').then(o => o.AccountModule),
  },
  {
    path: 'admin',
    loadChildren: () => import('./admin/admin.module').then(o => o.AdminModule),
  },
  {
    path: '**',
    redirectTo: 'admin',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
