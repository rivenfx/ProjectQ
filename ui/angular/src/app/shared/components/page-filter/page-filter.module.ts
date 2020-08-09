import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PageFilterComponent } from './page-filter';
import { ServiceProxyModule } from '@service-proxies';
import { SharedModule } from '@shared';
import { SampleComponentsModule } from '@shared/components/sample-components';


@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    SampleComponentsModule
  ],
  declarations: [
    PageFilterComponent
  ],
  exports: [
    PageFilterComponent
  ],
})
export class PageFilterModule {
}
