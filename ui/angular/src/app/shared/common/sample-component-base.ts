import { Injector, Input } from '@angular/core';
import { I18nService } from '@core/i18n';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { PermissionCheckerService } from '@shared/riven/permission-checker.service';

export abstract class SampleComponentBase {

  @Input() loading: boolean;

  i18nSer: I18nService;

  permission: PermissionCheckerService;

  constructor(
    public injector: Injector,
  ) {
    this.i18nSer = injector.get<I18nService>(ALAIN_I18N_TOKEN);
  }

  l(key: string, ...args: any[]): string {
    return this.i18nSer.fanyi(key, args);
  }

  isGranted(permissionName: string | string[]): boolean {
    if (!permissionName) {
      return true;
    }

    return this.permission.isGranted(permissionName);
  }
}
