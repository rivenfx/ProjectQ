import { ChangeDetectionStrategy, Component, Injector, Input, OnInit } from '@angular/core';
import { SampleComponentBase } from '@shared/common';
import { numberToChinese } from '@delon/abc';

@Component({
  selector: 'sample-form-item,[sample-form-item]',
  templateUrl: './sample-form-item.component.html',
  styleUrls: ['./sample-form-item.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SampleFormItemComponent extends SampleComponentBase
  implements OnInit {

  @Input() name: string;

  @Input() label: string;

  @Input() tips: string = '';

  @Input() required: boolean;


  // @Input() nzXs = 24;
  //
  // @Input() nzSm = 12;
  //
  // @Input() nzMd: number;
  //
  // @Input() nzLg: number;
  //
  // @Input() nzXl: number;
  //
  // @Input() nzXXl: number;

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  ngOnInit(): void {
  }

}
