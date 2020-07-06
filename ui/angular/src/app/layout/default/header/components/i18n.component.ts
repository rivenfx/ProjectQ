import { DOCUMENT } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, Injector, Input } from '@angular/core';
import { ALAIN_I18N_TOKEN, SettingsService } from '@delon/theme';
import { InputBoolean } from '@delon/util';

import { I18nService } from '@core/i18n';
import { AppConsts, SampleComponentBase } from '@shared';

@Component({
  selector: 'header-i18n',
  template: `
    <div *ngIf="showLangText" nz-dropdown [nzDropdownMenu]="langMenu" nzPlacement="bottomRight" class="alain-default__nav-item">
      <i nz-icon nzType="global" class="alain-default__nav-item-icon"></i>
<!--      {{ curLang.displayName }}-->
<!--      <i nz-icon nzType="down"></i>-->
    </div>
    <i
      *ngIf="!showLangText"
      nz-dropdown
      [nzDropdownMenu]="langMenu"
      nzPlacement="bottomRight"
      nz-icon
      nzType="global"
    ></i>
    <nz-dropdown-menu #langMenu="nzDropdownMenu">
      <ul nz-menu>
        <li
          nz-menu-item
          *ngFor="let item of langs"
          [nzSelected]="item.culture === curLangCulture"
          (click)="change(item.culture)"
        >
          <!-- <span role="img" [attr.aria-label]="item.text" class="pr-xs">{{ item.abbr }}</span>-->
          {{ item.displayName }}
        </li>
      </ul>
    </nz-dropdown-menu>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderI18nComponent extends SampleComponentBase {
  /** Whether to display language text */
  @Input() @InputBoolean() showLangText = true;

  get langs() {
    return this.i18nSer.getLangs();
  }

  get curLangCulture() {
    return this.curLang.culture;
  }

  get curLang(){
   return  this.i18nSer.curLang;
  }

  constructor(
    injector: Injector,
    private cdr: ChangeDetectorRef,
  ) {
    super(injector);
    this.i18nSer.change.subscribe((lang) => {
      this.cdr.detectChanges();
    });
  }

  change(lang: string) {
    this.i18nSer.use(lang);
  }
}
