import { Injector, Directive, Component } from '@angular/core';
import { TitleService } from '@delon/theme';
import { AppConsts } from '@shared/app-consts';
import { MessageService } from '@shared/riven/message.service';
import { NotifyService } from '@shared/riven/notify.service';
import { ComponentBase } from '@rivenfx/ng-common';


@Component({
  template: '',
})
// tslint:disable-next-line:component-class-suffix
export abstract class AppComponentBase extends ComponentBase {

  appConsts = AppConsts;

  constructor(public injector: Injector) {
    super(injector);
  }
}
