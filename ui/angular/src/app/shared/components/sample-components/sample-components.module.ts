import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SampleInputComponent } from '@shared/components/sample-components/sample-input';
import { SHARED_ZORRO_MODULES } from '@shared/shared-zorro.module';
import { SampleDataSourceService } from './sample-data-source.service';

const COMPONENTS = [
  SampleInputComponent,
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SHARED_ZORRO_MODULES
  ],
  declarations: [
    ...COMPONENTS,
  ],
  exports: [
    ...COMPONENTS,
  ],
  providers: [
    SampleDataSourceService
  ]
})
export class SampleComponentsModule {
}
