import { Inject, Injector } from '@angular/core';
import { I18nService } from '@core/i18n';
import { ALAIN_I18N_TOKEN, TitleService } from '@delon/theme';
import { AppConsts } from '@shared/app-consts';
import { MessageService } from '@shared/riven/message.service';
import { SampleComponentBase } from './sample-component-base';
import { NotifyService } from '@shared/riven';

export abstract class AppComponentBase extends SampleComponentBase {

  titleSer: TitleService;

  appConsts = AppConsts;

  message: MessageService;
  notify: NotifyService;


  constructor(
    public injector: Injector,
  ) {
    super(injector);

    this.titleSer = injector.get(TitleService);
    this.message = injector.get(MessageService);
    this.notify = injector.get(NotifyService);
  }
}
