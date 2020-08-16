import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { SHARED_DELON_MODULES } from '@shared/shared-delon.module';
import { SHARED_ZORRO_MODULES } from '@shared/shared-zorro.module';
import { SampleDataSourceService } from './sample-data-source.service';
import { SampleInputComponent } from './sample-input';
import { SampleSelectComponent } from './sample-select';
import { SampleTableComponent } from './sample-table';
import { SampleTableDataProcessorService } from './sample-table-data-processor.service';


const COMPONENTS = [
  SampleInputComponent,
  SampleSelectComponent,
  SampleTableComponent,
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SHARED_DELON_MODULES,
    SHARED_ZORRO_MODULES,
  ],
  declarations: [
    ...COMPONENTS,
  ],
  exports: [
    ...COMPONENTS,
  ],
  providers: [
    SampleDataSourceService,
    SampleTableDataProcessorService,
  ],
})
export class SampleComponentsModule {
}
