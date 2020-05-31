// 请参考：https://ng-alain.com/docs/i18n
import { registerLocaleData } from '@angular/common';
import { Injectable } from '@angular/core';
import {
  AlainI18NService,
  DelonLocaleService,
  SettingsService,
} from '@delon/theme';
import { NzI18nService } from 'ng-zorro-antd/i18n';
import { BehaviorSubject, Observable } from 'rxjs';
import { filter } from 'rxjs/operators';
import { I18nCommon } from './i18n-common';
import { AppConsts } from '@shared';
import { SessionService } from '../../shared/riven';


@Injectable({ providedIn: 'root' })
export class I18nService implements AlainI18NService {

  private _replaceSearchValue = /[.*+?^${}()|[\]\\]/g;
  private _replaceValue = '\\$&';
  private _relaceFlags = 'g';

  private change$ = new BehaviorSubject<string | null>(null);

  constructor(
    private settings: SettingsService,
    private nzI18nService: NzI18nService,
    private delonLocaleService: DelonLocaleService,
    private sessionSer: SessionService,
  ) {

  }

  /** 更新语言数据 */
  private updateLangData(lang: string) {
    const item = I18nCommon.LANG_MAP[lang];
    registerLocaleData(item.ng, item.abbr);
    this.nzI18nService.setLocale(item.zorro);
    this.nzI18nService.setDateLocale(item.date);
    this.delonLocaleService.setLocale(item.delon);
  }

  /** 语言发生更改的发布者 */
  get change(): Observable<string> {
    return this.change$.asObservable().pipe(filter((w) => w != null)) as Observable<string>;
  }

  /** 修改使用的语言 */
  use(lang: string): void {
    if (lang === this.sessionSer.session.localization.current.culture) {
      return;
    }

    this.settings.setLayout(AppConsts.settings.lang, lang);
    this.sessionSer.loadLocalization()
      .subscribe(() => {
        this.updateLangData(lang);
        this.change$.next(lang);
      });
  }

  /** 获取语言列表 */
  getLangs() {
    return this.sessionSer.session.localization.languages;
  }

  /**
   * 翻译
   * @param key 键值
   * @param interpolateParams 模板参数
   */
  fanyi(key: string, interpolateParams?: {}) {
    if (!this.sessionSer.session
      || !this.sessionSer.session.localization
      || !this.sessionSer.session.localization.current
      || !this.sessionSer.session.localization.current.texts) {
      return key;
    }


    let value = this.sessionSer.session.localization.current.texts[key];
    if (!value) {
      return key;
    }

    if (Array.isArray(interpolateParams) && interpolateParams.length > 0) {
      value = this.formatString(value, interpolateParams);
    }

    return value;
  }

  /**
   * 格式化本地化文字
   * @param inputStr
   * @param args
   */
  private formatString(inputStr: string, args: any[]) {
    if (!args || args.length < 1) {
      return inputStr;
    }


    for (let i = 0; i < args.length; i++) {
      const placeHolder = '{' + i + '}';
      const fix = placeHolder.replace(this._replaceSearchValue, this._replaceValue);
      inputStr = inputStr.replace(new RegExp(fix, this._relaceFlags), args[i]);
    }

    return inputStr;
  }
}
