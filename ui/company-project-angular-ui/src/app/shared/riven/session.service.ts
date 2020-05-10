import { Injectable } from '@angular/core';
import { ICurrentUserInfo } from './interfaces';
import { SessionDto, SessionServiceProxy } from '../../service-proxies';
import { catchError, finalize } from 'rxjs/operators';

@Injectable()
export class SessionService {
  private _session: SessionDto;

  get session(): SessionDto {
    return this._session;
  }

  constructor(
    private sessionSrv: SessionServiceProxy,
  ) {
  }


  /**
   * 加载或更新AppInfo
   */
  loadOrUpdateAppInfo(callback?: (state: boolean) => void) {
    this.sessionSrv.getCurrentSession()
      .pipe(catchError(err => {
        return err;
      }))
      .subscribe((res) => {
        if (res instanceof SessionDto) {
          this._session = res;
          callback(true);
        } else {
          callback(false);
        }
      });
  }
}
