import { Injectable } from '@angular/core';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Injectable()
export class NotifyService {

  constructor(
    public notification: NzNotificationService,
  ) {
  }

  info(message: string, title?: string, options?: any): void {
    this.notification.info(message, title, options);
  }

  success(message: string, title?: string, options?: any): void {
    this.notification.success(message, title, options);
  }

  warn(message: string, title?: string, options?: any): void {
    this.notification.warning(message, title, options);
  }

  error(message: string, title?: string, options?: any): void {
    this.notification.error(message, title, options);
  }

}
