import { NgModule } from '@angular/core';
import { DelonFormModule, WidgetRegistry } from '@delon/form';

import { SharedModule } from '@shared/shared.module';
import { SfWidgetModule } from './sf-widgets';
import { StWidgetModule } from './st-widgets/st-widgets.module';

@NgModule({
  imports: [SharedModule, DelonFormModule.forRoot()],
  exports: [SfWidgetModule, StWidgetModule],
})
export class JsonSchemaModule {
  constructor() {}
}
