import { NZ_DATE_LOCALE, NZ_I18N } from 'ng-zorro-antd/i18n';

//
import { LOCALE_ID } from '@angular/core';
import { default as ngLang } from '@angular/common/locales/zh';
import { en_US as zorroEnUS, zh_CN as zorroLang } from 'ng-zorro-antd/i18n';
import { enUS as dfEn, zhCN as dateLang } from 'date-fns/locale';
import { en_US as delonEnUS, zh_CN as delonLang } from '@delon/theme';
import ngEn from '@angular/common/locales/en';
import { DELON_LOCALE } from '@delon/theme';
import { ILangData } from './interfaces';

export class I18nCommon {

  /** 默认语言 */
  static readonly DEFAULT_LANG: ILangData = {
    text: '简体中文',
    abbr: 'zh-Hans',
    ng: ngLang,
    zorro: zorroLang,
    date: dateLang,
    delon: delonLang,
  };

  /** 语言提供者 */
  static readonly LANG_PROVIDES = [
    { provide: LOCALE_ID, useValue: I18nCommon.DEFAULT_LANG.abbr },
    { provide: NZ_I18N, useValue: I18nCommon.DEFAULT_LANG.zorro },
    { provide: NZ_DATE_LOCALE, useValue: I18nCommon.DEFAULT_LANG.date },
    { provide: DELON_LOCALE, useValue: I18nCommon.DEFAULT_LANG.delon },
  ];

  /** 语言映射字典 */
  static readonly LANG_MAP: { [key: string]: ILangData } = {
    'zh-Hans': I18nCommon.DEFAULT_LANG,
    'en': {
      text: 'English',
      abbr: 'en',
      ng: ngEn,
      zorro: zorroEnUS,
      date: dfEn,
      delon: delonEnUS,
    },
  };
}
