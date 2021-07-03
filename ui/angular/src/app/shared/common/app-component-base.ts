import { Injector, Directive, Component } from '@angular/core';
import { ComponentBase } from '@rivenfx/ng-common';


@Component({
  template: '',
})
// tslint:disable-next-line:component-class-suffix
export abstract class AppComponentBase extends ComponentBase {

  constructor(public injector: Injector) {
    super(injector);
  }
}
