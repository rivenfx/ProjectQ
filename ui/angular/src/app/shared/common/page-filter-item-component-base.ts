import { ControlComponentBase } from '@shared/common/control-component-base';
import { ChangeDetectorRef, Injector, Input, SimpleChange, SimpleChanges } from '@angular/core';

/** page filter 组件的基类 */
export abstract class PageFilterItemComponentBase<T> extends ControlComponentBase<T> {

  /** 配置文件中的参数 */
  @Input() args: string;

  /** 外部组件传递的参数 */
  @Input() externalArgs: any = {};

  cdr: ChangeDetectorRef;

  constructor(
    injector: Injector,
  ) {
    super(injector);

    this.cdr = injector.get(ChangeDetectorRef);
  }

  // ngOnChanges(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges): void {
  //   super.ngOnChanges(changes);
  //   debugger
  //   if (changes.args && changes.args.currentValue) {
  //     this.onArgsChange(JSON.parse(changes.args.currentValue));
  //   }
  //   if (changes.externalArgs && changes.externalArgs.currentValue) {
  //     this.onExternalArgs(changes.externalArgs.currentValue);
  //   }
  // }

  /** 参数发生更改 */
  abstract onArgsChange(args: any);

  /** 外部组件传递的参数发生过更改 */
  abstract onExternalArgs(externalArgs: any);
}

