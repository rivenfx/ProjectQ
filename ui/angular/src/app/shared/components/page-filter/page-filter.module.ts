import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '@shared';
import { SampleComponentsModule } from '@shared/components/sample-components';
import { PageFilterComponent } from './page-filter';


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
