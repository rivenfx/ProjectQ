import { ModuleWithProviders, NgModule } from '@angular/core';
import { I18nPipe } from './i18n.pipe';
import { I18nService } from './i18n.service';


@NgModule({
  imports: [],
  declarations: [
    I18nPipe,
  ],
  exports: [
    I18nPipe,
  ],
})
export class I18nModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: I18nModule,
      providers: [
        I18nService,
      ],
    };
  }
}
