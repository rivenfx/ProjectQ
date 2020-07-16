import { ALAIN_I18N_TOKEN, TitleService } from '@delon/theme';
import { I18nService } from '@core/i18n';
import { Inject, Injector } from '@angular/core';
import { SampleComponentBase } from './sample-component-base';
import { AppConsts } from '@shared/app-consts';

export abstract class AppComponentBase extends SampleComponentBase {

  loading: boolean;

  titleSer: TitleService;

  appConsts = AppConsts;

  constructor(
    public injector: Injector,
  ) {
    super(injector);

    this.titleSer = injector.get(TitleService);
  }
}
