import { ChangeDetectionStrategy, Component, HostListener, Injector } from '@angular/core';
import { SampleComponentBase } from '@shared/common';
import * as screenfull from 'screenfull';

@Component({
  selector: 'header-fullscreen',
  template: `
    <i nz-icon [nzType]="status ? 'fullscreen-exit' : 'fullscreen'"></i>
    {{ l(status ? 'menu.fullscreen.exit' : 'menu.fullscreen') }}
  `,
  // tslint:disable-next-line: no-host-metadata-property
  host: {
    '[class.d-block]': 'true',
  },
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderFullScreenComponent extends SampleComponentBase {
  status = false;

  private get sf(): screenfull.Screenfull {
    return screenfull as screenfull.Screenfull;
  }

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  @HostListener('window:resize')
  _resize() {
    this.status = this.sf.isFullscreen;
  }

  @HostListener('click')
  _click() {
    if (this.sf.isEnabled) {
      this.sf.toggle();
    }
  }
}
