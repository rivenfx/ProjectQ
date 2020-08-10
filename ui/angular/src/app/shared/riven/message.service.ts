import { Inject, Injectable } from '@angular/core';
import { I18nService } from '@core/i18n/i18n.service';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { NzModalService } from 'ng-zorro-antd/modal';

@Injectable()
export class MessageService {
  constructor(
    public nzModalSer: NzModalService,
    @Inject(ALAIN_I18N_TOKEN) public i18n: I18nService,
  ) {

  }


  info(message: string, title?: string, isHtml?: boolean): any {
    if (title) {
      return this.nzModalSer.info({
        nzTitle: title,
        nzContent: message,
      });
    } else {
      return this.nzModalSer.info({
        nzTitle: message,
      });
    }
  }

  success(message: string, title?: string, isHtml?: boolean): any {
    if (title) {
      return this.nzModalSer.success({
        nzTitle: title,
        nzContent: message,
      });
    } else {
      return this.nzModalSer.success({
        nzTitle: message,
      });
    }
  }

  warn(message: string, title?: string, isHtml?: boolean): any {
    if (title) {
      return this.nzModalSer.warning({
        nzTitle: title,
        nzContent: message,
      });
    } else {
      return this.nzModalSer.warning({
        nzTitle: message,
      });
    }
  }

  error(message: string, title?: string, isHtml?: boolean): any {
    if (title) {
      return this.nzModalSer.error({
        nzTitle: title,
        nzContent: message,
      });
    } else {
      return this.nzModalSer.error({
        nzTitle: message,
      });
    }
  }

  confirm(message: string, callback?: (result: boolean) => void): any;
  confirm(message: string, title?: string, callback?: (result: boolean) => void, isHtml?: boolean): any;

  confirm(message: string, titleOrCallBack?: string | ((result: boolean) => void), callback?: (result: boolean) => void, isHtml?: boolean): any {
    if (typeof titleOrCallBack === 'string') {
      this.nzModalSer.confirm({
        nzTitle: titleOrCallBack,
        nzContent: message,
        nzOnOk() {
          if (callback) {
            callback(true);
          }
        },
        nzOnCancel() {
          if (callback) {
            callback(false);
          }
        },
      });
    } else {
      this.nzModalSer.confirm({
        nzTitle: this.i18n.fanyi('MessageConfirmOperation'),
        nzContent: message,
        nzOnOk() {
          if (titleOrCallBack) {
            titleOrCallBack(true);
          }
        },
        nzOnCancel() {
          if (titleOrCallBack) {
            titleOrCallBack(false);
          }
        },
      });
    }
  }
}
