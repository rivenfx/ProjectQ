import { ChangeDetectionStrategy, Component, Injector } from '@angular/core';
import { SettingsService } from '@delon/theme';
import { SampleComponentBase } from '@shared/common';

@Component({
  selector: 'layout-sidebar',
  templateUrl: './sidebar.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent extends SampleComponentBase {
  constructor(
    injector: Injector,
    public settings: SettingsService,
  ) {
    super(injector);
  }
}
