import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { TableBarComponent } from './table-bar';


@NgModule({
  imports: [
    CommonModule,
  ],
  declarations: [
    TableBarComponent,
  ],
  exports: [
    TableBarComponent,
  ],
})
export class TableBarModule {
}
