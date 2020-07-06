import { Inject, Pipe, PipeTransform } from '@angular/core';
import { I18nService } from './i18n.service';
import { ALAIN_I18N_TOKEN } from '@delon/theme';


@Pipe({ name: 'l' })
export class I18nPipe implements PipeTransform {
  constructor(
    @Inject(ALAIN_I18N_TOKEN) private i18nSer: I18nService,
  ) {
  }

  transform(value: any, ...args: any[]): any {
    return this.i18nSer.fanyi(value, args);
  }
}
