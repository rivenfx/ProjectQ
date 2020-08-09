import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SampleInputComponent } from '@shared/components/sample-components/sample-input';
import { SHARED_ZORRO_MODULES } from '@shared/shared-zorro.module';
import { FormsModule } from '@angular/forms';

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
})
export class SampleComponentsModule {
}
