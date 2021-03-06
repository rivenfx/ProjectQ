import { Injector, Input } from '@angular/core';
import { I18nService } from '@core/i18n';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { PermissionCheckerService } from '@shared/riven/permission-checker.service';

export abstract class SampleComponentBase {

  @Input() loading: boolean;

  i18nSer: I18nService;

  permissionSer: PermissionCheckerService;

  constructor(
    public injector: Injector,
  ) {
    this.i18nSer = injector.get<I18nService>(ALAIN_I18N_TOKEN);
    this.permissionSer = injector.get(PermissionCheckerService);
  }

  l(key: string, ...args: any[]): string {
    return this.i18nSer.fanyi(key, args);
  }

  isGranted(permissionName: string | string[], requireAll: boolean = false): boolean {
    if (!permissionName) {
      return true;
    }

    return this.permissionSer.isGranted(permissionName, requireAll);
  }
}
