import {
  ChangeDetectionStrategy,
  Component,
  forwardRef,
  Injector,
  OnInit,
  SimpleChange,
  SimpleChanges,
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { PageFilterItemComponentBase } from '@rivenfx/ng-page-filter';
import * as _ from 'lodash';
import { SampleDataSourceService } from '../sample-data-source.service';

@Component({
  selector: 'sample-select',
  templateUrl: './sample-select.component.html',
  styleUrls: ['./sample-select.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SampleSelectComponent),
    multi: true,
  }],
})
export class SampleSelectComponent extends PageFilterItemComponentBase {

  argsObject: {
    dataSource: undefined,
    type: undefined, // default/mulit/tree/tree-mulit
    placeholder: undefined,
    maxCount: 3
  };

  souceData: any[] = [];

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
    this.argsObject = _.merge(this.argsObject, args);
    if (this.argsObject.placeholder) {
      this.placeholder = this.l(this.argsObject.placeholder);
    }

    const obs = this.sampleDataSourceSer.fetchData<any>(this.argsObject.dataSource, this.args);
    if (obs) {
      obs.subscribe((res) => {
        this.souceData = res;
        this.cdr.detectChanges();
        this.imReady();
      });
    }
    this.cdr.detectChanges();
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
