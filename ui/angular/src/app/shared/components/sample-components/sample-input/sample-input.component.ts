import { ChangeDetectionStrategy, Component, Injector, SimpleChange, SimpleChanges } from '@angular/core';
import { PageFilterItemComponentBase } from '@shared/common';

@Component({
  selector: 'sample-input',
  templateUrl: './sample-input.component.html',
  styleUrls: ['./sample-input.component.less'],
  // changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SampleInputComponent extends PageFilterItemComponentBase<any> {


  argsObject: {
    type: undefined,
    placeholder: undefined,
    min: undefined,
    max: undefined,
    step: undefined,
  };

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  ngOnInit(): void {
  }

  onAfterViewInit(): void {
  }

  onArgsChange(args: any) {
    this.argsObject = args;
    if (this.argsObject.placeholder) {
      this.placeholder = this.l(this.argsObject.placeholder);
    }
    debugger;
    this.cdr.detectChanges();
  }

  onExternalArgs(externalArgs: any) {

  }

  onDestroy(): void {
  }

  onInit(): void {
  }

  onInputChange(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges) {
  }

}
