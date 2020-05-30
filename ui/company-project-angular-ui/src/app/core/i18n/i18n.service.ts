// 请参考：https://ng-alain.com/docs/i18n
import { registerLocaleData } from '@angular/common';
import ngEn from '@angular/common/locales/en';
import ngZh from '@angular/common/locales/zh';
import ngZhTw from '@angular/common/locales/zh-Hant';
import { Injectable } from '@angular/core';
import {
  AlainI18NService,
  DelonLocaleService,
  SettingsService,
} from '@delon/theme';
import { TranslateService } from '@ngx-translate/core';
import { NzI18nService } from 'ng-zorro-antd/i18n';
import { BehaviorSubject, Observable } from 'rxjs';
import { filter } from 'rxjs/operators';
import { I18nCommon } from './i18n-common';
import { ILang } from './interfaces';
import { LocalizationDto } from '../../service-proxies';
import { AppConsts } from '@shared';


@Injectable({ providedIn: 'root' })
export class I18NService implements AlainI18NService {
  private _default = I18nCommon.DEFAULT_LANG.abbr;
  private change$ = new BehaviorSubject<string | null>(null);

  private _langs: ILang[] = [];
  private _inited = false;

  constructor(
    private settings: SettingsService,
    private nzI18nService: NzI18nService,
    private delonLocaleService: DelonLocaleService,
    private translate: TranslateService,
  ) {

  }

  /**
   * 初始化多语言
   * @param input 多语言数据
   */
  init(input: LocalizationDto) {
    if (this._inited) {
      return;
    }
    this._inited = true;

    this._default = input.default.culture;

    // 所有语言编码
    const langCodes = input.languages.map(o => o.culture);
    langCodes.push(input.current.culture);

    // 添加到 ngx-translate 中
    this.translate.addLangs(langCodes);
    this.translate.setTranslation(input.current.culture, input.current.texts);
    this.translate.setDefaultLang(this.defaultLang);

    // 更新当前组件依赖的语言数据
    if (input.current.culture !== this.defaultLang) {
      this.updateLangData(input.current.culture);
    }

    // 更新程序内使用的语言配置
    this._langs = [];
    this._langs = input.languages.map<ILang>(o => {
      return {
        code: o.culture,
        text: o.displayName,
        icon: o.icon,
        abbr: I18nCommon.LANG_MAP[o.culture].abbr,
      };
    });
    this._langs.push({
      code: input.current.culture,
      text: input.current.displayName,
      icon: input.current.icon,
      abbr: I18nCommon.LANG_MAP[input.current.culture].abbr,
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

  /** 修改使用的语言 */
  use(lang: string): void {
    this.settings.setLayout(AppConsts.settings.lang, lang);

    lang = lang || this.translate.getDefaultLang();
    if (this.currentLang === lang) {
      return;
    }
    this.updateLangData(lang);
    this.translate.use(lang).subscribe(() => {
      this.change$.next(lang);
    });
  }

  /** 获取语言列表 */
  getLangs() {
    return this._langs;
  }

  /** 翻译 */
  fanyi(key: string, interpolateParams?: {}) {
    return this.translate.instant(key, interpolateParams);
  }

  /** 默认语言 */
  get defaultLang() {
    return this._default;
  }

  /** 当前语言 */
  get currentLang() {
    return this.translate.currentLang || this.translate.getDefaultLang() || this._default;
  }
}
