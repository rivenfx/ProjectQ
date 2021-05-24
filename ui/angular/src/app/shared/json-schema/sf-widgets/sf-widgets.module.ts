import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { WidgetRegistry } from '@delon/form';
import { SfMomentComponent } from './sf-moment';

export const SFWIDGET_COMPONENTS = [SfMomentComponent];

@NgModule({
  imports: [SharedModule],
  declarations: [...SFWIDGET_COMPONENTS],

  exports: [...SFWIDGET_COMPONENTS],
})
export class SfWidgetModule {
  constructor(widgetRegistry: WidgetRegistry) {
    widgetRegistry.register(SfMomentComponent.KEY, SfMomentComponent);
  }
}
