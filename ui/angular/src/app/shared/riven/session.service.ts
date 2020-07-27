import { Injectable } from '@angular/core';
import {
  LanguageInfoDto,
  LocalizationDto,
  SessionDto,
  SessionServiceProxy,
} from '../../service-proxies/service-proxies';
import { BehaviorSubject, Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

@Injectable()
export class SessionService {

  private _sessionChange$ = new BehaviorSubject<SessionDto | null>(null);
  private localizationChange$ = new BehaviorSubject<LocalizationDto | null>(null);


  private _session: SessionDto;


  constructor(
    private sessionSrv: SessionServiceProxy,
  ) {
  }


  get session(): SessionDto {
    return this._session;
  }

  get sessionChange(): Observable<SessionDto> {
    return this._sessionChange$.asObservable();
  }

  get localizationChange(): Observable<LocalizationDto | null> {
    return this.localizationChange$.asObservable();
  }

  /**
   * 加载或更新AppInfo
   */
  loadOrUpdateAppInfo(callback?: (state: boolean, data: SessionDto | any) => void) {
    this.sessionSrv.getCurrentSession()
      .subscribe({
        next: (res) => {
          this._session = res;
          this.localizationChange$.next(this._session.localization);
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
        this.localizationChange$.next(this.session.localization);
        obs.next(this.session.localization);
        obs.complete();
      } else {
        this.sessionSrv.getLocalization()
          .pipe(finalize(() => {
            obs.complete();
          }))
          .subscribe((res) => {
            this.session.localization = res;
            this.localizationChange$.next(this.session.localization);
            obs.next(this.session.localization);
          });
      }
    });
  }
}
