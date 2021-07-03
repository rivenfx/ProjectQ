import { Injectable, Injector } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import {
  LocalizationDto,
  SessionDto,
  SessionServiceProxy,
} from '@service-proxies';
import { LazyInit } from '@shared/utils';

@Injectable()
export class SessionService {

  private _sessionChange$ = new BehaviorSubject<SessionDto | null>(null);
  private _localizationChange$ = new BehaviorSubject<LocalizationDto | null>(null);

  private _sessionSrv: LazyInit<SessionServiceProxy>;
  private _session: SessionDto;



  constructor(
    injector: Injector
  ) {
    this._sessionSrv = new LazyInit<SessionServiceProxy>(() => {
      return injector.get(SessionServiceProxy);
    });
  }


  get session(): SessionDto {
    return this._session;
  }

  get sessionChange(): Observable<SessionDto> {
    return this._sessionChange$.asObservable();
  }

  get localizationChange(): Observable<LocalizationDto | null> {
    return this._localizationChange$.asObservable();
  }

  /**
   * 加载或更新AppInfo
   */
  loadOrUpdateAppInfo(callback?: (state: boolean, data: SessionDto | any) => void) {
    this._sessionSrv.instance.getCurrentSession()
      .subscribe({
        next: (res) => {
          this._session = res;
          this._localizationChange$.next(this._session.localization);
          this._sessionChange$.next(this._session);
          callback(true, this._session);
        },
        error: (error) => {
          callback(false, error);
        },
      });
  }

  /** 加载本地化资源 */
  loadLocalization(culture?: string): Observable<LocalizationDto | null> {
    return new Observable<LocalizationDto | null>((obs) => {
      if (this.session
        && this.session.localization
        && this.session.localization.currentCulture
        && this.session.localization.currentCulture === culture) {
        this._localizationChange$.next(this.session.localization);
        obs.next(this.session.localization);
        obs.complete();
      } else {
        this._sessionSrv.instance.getLocalization()
          .pipe(finalize(() => {
            obs.complete();
          }))
          .subscribe((res) => {
            this.session.localization = res;
            this._localizationChange$.next(this.session.localization);
            obs.next(this.session.localization);
          });
      }
    });
  }
}
