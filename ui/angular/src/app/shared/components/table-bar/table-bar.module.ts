import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

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
