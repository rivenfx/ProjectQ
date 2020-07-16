import { Injector } from '@angular/core';
import { AppComponentBase } from './app-component-base';

export abstract class ListViewComponentBase extends AppComponentBase {

  constructor(injector: Injector) {
    super(injector);
  }

}
