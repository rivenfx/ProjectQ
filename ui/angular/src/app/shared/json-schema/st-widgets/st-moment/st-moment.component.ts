import { ChangeDetectionStrategy, Component } from '@angular/core';
import * as moment from 'moment';

// @ts-ignore
@Component({
  selector: 'st-widget-moment',
  templateUrl: './st-moment.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StMomentComponent {
  static readonly KEY = 'moment';

  date: moment.Moment;

  format = 'YYYY-MM-DD';

  constructor() {}
}
