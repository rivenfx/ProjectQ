import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class SampleDataSourceService {

  constructor() {

  }


  /** 根据数据源名称和参数返回对应的数据 */
  fetchData<TData>(dataSourceName: string, args?: any): Observable<TData> {
    switch (dataSourceName) {
      case 'yesOrNo':
        return this.yesOrNo();
    }

    return undefined;
  }


  private yesOrNo(): Observable<any> {
    return new Observable<any>((obs) => {
      obs.next([
        { label: 'label.yes', value: true },
        { label: 'label.no', value: false },
      ]);
      obs.complete();
    });
  }
}
