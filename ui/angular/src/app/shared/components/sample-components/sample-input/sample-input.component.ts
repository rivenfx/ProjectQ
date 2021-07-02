import { ChangeDetectionStrategy, Component, forwardRef, Injector, SimpleChange, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { PageFilterItemComponentBase } from '@rivenfx/ng-page-filter';
import { SampleDataSourceService } from '../sample-data-source.service';


@Component({
  selector: 'sample-input',
  templateUrl: './sample-input.component.html',
  styleUrls: ['./sample-input.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SampleInputComponent),
    multi: true,
  }],
})
export class SampleInputComponent extends PageFilterItemComponentBase {

  argsObject: {
    type: undefined,
    placeholder: undefined,
    min: undefined,
    max: undefined,
    step: undefined,
  };

  sampleDataSourceSer: SampleDataSourceService;

  constructor(
    injector: Injector,
  ) {
    super(injector);
    this.sampleDataSourceSer = injector.get(SampleDataSourceService);
  }

  onAfterViewInit(): void {
  }

  onArgsChange(args: any) {
    this.argsObject = args;
    if (this.argsObject.placeholder) {
      this.placeholder = this.l(this.argsObject.placeholder);
    }
    this.cdr.detectChanges();
    this.imReady();
  }

  onExternalArgsChange(externalArgs: any) {

  }


  onDestroy(): void {
  }

  onInit(): void {
  }

  onInputChange(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges) {
  }

}
