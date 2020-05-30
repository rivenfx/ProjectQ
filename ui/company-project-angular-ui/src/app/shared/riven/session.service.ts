import { Injectable } from '@angular/core';
import { LocalizationDto, SessionDto, SessionServiceProxy } from '../../service-proxies';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

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
          callback(false, error);
        },
      });
  }

  /** 加载本地化资源 */
  loadLocalization(lang?: string): Observable<{ [key: string]: string }> {
    debugger
    return new Observable<{ [key: string]: string }>((obs) => {
      if (this.session
        && this.session.localization
        && this.session.localization.current
        && this.session.localization.current.culture === lang) {
        obs.next(this.session.localization.current.texts);
        obs.complete();
      } else {
        this.sessionSrv.getLocalization()
          .pipe(finalize(() => {
            obs.complete();
          }))
          .subscribe((res) => {
            this._session.localization = res;
            obs.next(this.session.localization.current.texts);
          });
      }
    });
  }
}
