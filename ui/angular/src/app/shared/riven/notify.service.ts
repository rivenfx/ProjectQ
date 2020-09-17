import { Injectable } from '@angular/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import * as _ from 'loadsh';

@Injectable()
export class NotifyService {

  protected _options = {
    nzPlacement: 'bottomRight'
  };

  constructor(
    public notification: NzNotificationService,
  ) {
  }

  info(message: string, title?: string, options: any = {}): void {
    if (!options) {
      options = {};
    }
    this.notification.info(message, title, {
      ...options,
      ...this._options
    });
  }

  success(message: string, title?: string, options: any = {}): void {
    if (!options) {
      options = {};
    }
    this.notification.success(message, title, {
      ...options,
      ...this._options
    });
  }

  warn(message: string, title?: string, options: any = {}): void {
    if (!options) {
      options = {};
    }
    this.notification.warning(message, title, {
      ...options,
      ...this._options
    });
  }

  error(message: string, title?: string, options: any = {}): void {
    if (!options) {
      options = {};
    }
    this.notification.error(message, title, {
      ...options,
      ...this._options
    });
  }

}
