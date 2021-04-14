import { Injector, Directive } from '@angular/core';
import { TitleService } from '@delon/theme';
import { AppConsts } from '@shared/app-consts';
import { MessageService } from '@shared/riven/message.service';
import { NotifyService } from '@shared/riven/notify.service';
import { SampleComponentBase } from './sample-component-base';


@Directive()
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
