import { ALAIN_I18N_TOKEN, TitleService } from '@delon/theme';
import { I18nService } from '@core';
import { Inject, Injector } from '@angular/core';
import { SampleComponentBase } from './sample-component-base';

export abstract class AppComponentBase extends SampleComponentBase {

  titleSer: TitleService;

  constructor(
    public injector: Injector,
  ) {
    super(injector);

    this.titleSer = injector.get(TitleService);
  }
}
