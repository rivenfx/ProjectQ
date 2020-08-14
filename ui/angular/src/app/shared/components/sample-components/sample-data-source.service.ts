import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class SampleDataSourceService {

  constructor() {

  }


  /** 根据数据源名称和参数返回对应的数据 */
  fetchData<TData>(dataSourceName: string, args?: any): Observable<TData> {


    return undefined;
  }
}
