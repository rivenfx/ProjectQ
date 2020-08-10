// 请参考：https://ng-alain.com/docs/i18n
import { registerLocaleData } from '@angular/common';
import { Injectable } from '@angular/core';
import {
  AlainI18NService,
  DelonLocaleService,
  SettingsService,
} from '@delon/theme';

import { AppConsts } from '@shared/app-consts';
import { SessionService } from '@shared/riven/session.service';
import { NzI18nService } from 'ng-zorro-antd/i18n';
import { BehaviorSubject, Observable } from 'rxjs';
import { filter } from 'rxjs/operators';
import { LanguageInfoDto, LocalizationDto } from '../../service-proxies';
import { I18nCommon } from './i18n-common';


@Injectable({ providedIn: 'root' })
export class I18nService implements AlainI18NService {

  private _replaceSearchValue = /[.*+?^${}()|[\]\\]/g;
  private _replaceValue = '\\$&';
  private _relaceFlags = 'g';


  private change$ = new BehaviorSubject<string | null>(null);
  private _localization: LocalizationDto;
  private _curLang: LanguageInfoDto;

  constructor(
    private settings: SettingsService,
    private nzI18nService: NzI18nService,
    private delonLocaleService: DelonLocaleService,
    private sessionSer: SessionService,
  ) {
    this.sessionSer.localizationChange
      .subscribe((result) => {
        if (result){
          this._localization = result;
          this._curLang = result.languages.find(o => o.culture === result.currentCulture);
        }
      });
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

  /** 获取当前语言 */
  get curLang() {
    return this._curLang;
  }

  /** 获取语言列表 */
  getLangs() {
    if (!this._localization){
      return  [];
    }
    return this._localization.languages;
  }

  /** 修改使用的语言 */
  use(lang: string): void {
    this.settings.setLayout(AppConsts.settings.lang, lang);
    if (lang === this._localization.currentCulture) {
      return;
    }

    this.sessionSer.loadLocalization()
      .subscribe(() => {
        this.updateLangData(lang);
        this.change$.next(lang);
      });
  }

  /**
   * 翻译
   * @param key 键值
   * @param interpolateParams 模板参数
   */
  fanyi(key: string, interpolateParams?: {}) {
    if (!this._localization) {
      return key;
    }


    let value = this.curLang.texts[key];
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
   * @param inputStr 要格式化的文字
   * @param args 格式化的参数
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
