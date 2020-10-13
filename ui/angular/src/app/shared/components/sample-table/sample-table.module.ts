import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SampleTableComponent } from './sample-table.component';
import { FormsModule } from '@angular/forms';
import { SHARED_DELON_MODULES } from '@shared/shared-delon.module';
import { SHARED_ZORRO_MODULES } from '@shared/shared-zorro.module';
import { SampleTableDataProcessorService } from './sample-table-data-processor.service';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SHARED_DELON_MODULES,
    SHARED_ZORRO_MODULES,
  ],
  declarations: [
    SampleTableComponent
  ],
  exports: [
    SampleTableComponent
  ],
  providers: [
    SampleTableDataProcessorService
  ]
})
export class SampleTableModule {

}
