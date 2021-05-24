import { NgModule } from '@angular/core';
import { STWidgetRegistry } from '@delon/abc/st';
import { SharedModule } from '@shared/shared.module';
import { StMomentComponent } from './st-moment';

export const STWIDGET_COMPONENTS = [StMomentComponent];

@NgModule({
  imports: [SharedModule],
  declarations: [...STWIDGET_COMPONENTS],
  exports: [...STWIDGET_COMPONENTS],
})
export class StWidgetModule {
  constructor(widgetRegistry: STWidgetRegistry) {
    widgetRegistry.register(StMomentComponent.KEY, StMomentComponent);
  }
}
