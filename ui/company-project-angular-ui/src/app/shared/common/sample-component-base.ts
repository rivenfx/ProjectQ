import { I18NService } from '@core';
import { Injector } from '@angular/core';
import { ALAIN_I18N_TOKEN } from '@delon/theme';

export abstract class SampleComponentBase {


  i18nSer: I18NService;

  constructor(
    public injector: Injector,
  ) {
    this.i18nSer = injector.get<I18NService>(ALAIN_I18N_TOKEN);
  }
}
