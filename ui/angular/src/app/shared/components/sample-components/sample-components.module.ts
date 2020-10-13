import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { SHARED_DELON_MODULES } from '@shared/shared-delon.module';
import { SHARED_ZORRO_MODULES } from '@shared/shared-zorro.module';
import { SampleDataSourceService } from './sample-data-source.service';
import { SampleInputComponent } from './sample-input';
import { SampleSelectComponent } from './sample-select';

const COMPONENTS = [
  SampleInputComponent,
  SampleSelectComponent,
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
    SampleDataSourceService
  ],
})
export class SampleComponentsModule {
}
