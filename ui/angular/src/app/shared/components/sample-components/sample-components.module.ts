import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SampleInputComponent } from '@shared/components/sample-components/sample-input';
import { SHARED_ZORRO_MODULES } from '@shared/shared-zorro.module';

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
