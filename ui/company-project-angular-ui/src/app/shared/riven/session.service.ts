import { forwardRef, Inject, Injectable } from '@angular/core';
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
  loadOrUpdateAppInfo(callback?: (state: boolean, data: SessionDto | any) => void) {
    this.sessionSrv.getCurrentSession()
      .subscribe({
        next: (res) => {
          this._session = res;
          callback(true, this.session);
        },
        error: (error) => {
          callback(true, error);
        },
      });
  }
}
